using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace BookStore.API.Auth;

#region ClaimAuthorizeAttribute
public class ClaimAuthorizeAttribute : AuthorizeAttribute
{
    const string POLICY_PREFIX = "CLAIM_";
    readonly string _type;
    readonly string[] _values;

    public ClaimAuthorizeAttribute(string claimType, params string[] claimValues)
    {
        _type = claimType;
        _values = claimValues;

        if (claimValues.Length > 0)
        {
            Policy = $"{POLICY_PREFIX}<{_type}>{string.Join(",", _values)}";
        }
        else
        {
            Policy = $"{POLICY_PREFIX}<{_type}>";
        }
    }
}
#endregion

#region ClaimRequirement
public class ClaimRequirement : IAuthorizationRequirement
{
    public string ClaimType { get; }
    public string[] ClaimValues { get; }

    public ClaimRequirement(string claimType, params string[] claimValues)
    {
        ClaimType = claimType;
        ClaimValues = claimValues;
    }
}
#endregion

#region ClaimHandler
public class ClaimHandler : AuthorizationHandler<ClaimRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirement requirement)
    {
        if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
        {
            return Task.CompletedTask;
        }

        //檢查 User 是否有 IsAdmin=true 的 claim，有則直接賦予執行權限
        if (context.User.HasClaim(t => t.Type == "IsAdmin" && t.Value == "true"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var claimValues = requirement.ClaimValues.Where(s => !string.IsNullOrWhiteSpace(s))?.Distinct();
        if (claimValues?.Count() > 0)
        {
            if (context.User.HasClaim(t => t.Type == requirement.ClaimType && claimValues.Contains(t.Value)))
            {
                context.Succeed(requirement);
            }
        }
        else
        {
            if (context.User.HasClaim(t => t.Type == requirement.ClaimType))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
#endregion

#region ClaimPolicyProvider
public class ClaimPolicyProvider : IAuthorizationPolicyProvider
{
    //簡易的cache，將已經建立的 ClaimRequirement 存起來供後續使用
    static ConcurrentDictionary<string, AuthorizationPolicy> policyDict = new ConcurrentDictionary<string, AuthorizationPolicy>();

    const string CLAIM_POLICY_PREFIX = "CLAIM_";
    static readonly Regex rx = new Regex(@"CLAIM_\<(?'type'.*)\>(?'values'.*)$", RegexOptions.Compiled);

    private readonly string[] _authenticationSchemes;
    private DefaultAuthorizationPolicyProvider _backupPolicyProvider { get; }
    public ClaimPolicyProvider(
        IOptions<AuthorizationOptions> authorizationoptions,
        IOptions<ClaimAuthorizeOptions> claimAuthorizeoptions)
    {
        _authenticationSchemes = claimAuthorizeoptions.Value.AuthenticationSchemes;
        _backupPolicyProvider = new DefaultAuthorizationPolicyProvider(authorizationoptions);
    }

    //當僅有 [Authorize] 時會呼叫 GetDefaultPolicyAsync
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
        Task.FromResult(new AuthorizationPolicyBuilder(_authenticationSchemes).RequireAuthenticatedUser().Build());

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
        Task.FromResult<AuthorizationPolicy?>(null);

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(CLAIM_POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
        {
            if (!policyDict.TryGetValue(policyName, out AuthorizationPolicy? authorizationPolicy))
            {
                if (rx.IsMatch(policyName))
                {
                    var claimType = rx.Match(policyName).Groups["type"].Value;
                    var claimValues = rx.Match(policyName).Groups["values"].Value.Split(",");
                    var policy = new AuthorizationPolicyBuilder(_authenticationSchemes).RequireAuthenticatedUser();
                    policy.AddRequirements(new ClaimRequirement(claimType, claimValues));
                    authorizationPolicy = policy.Build();

                    policyDict.TryAdd(policyName, authorizationPolicy);
                }
            }
            return Task.FromResult(authorizationPolicy);
        }

        // 非 CLAIM_POLICY_PREFIX 開頭的則用 asp.net core 原生的 DefaultAuthorizationPolicyProvider
        return _backupPolicyProvider.GetPolicyAsync(policyName);
    }
}
#endregion

#region ClaimAuthorizeServiceCollectionExtension
public static class ClaimAuthorizeServiceCollectionExtension
{
    /// <summary>
    /// 加入 ClaimAuthoriztion 權限控管
    /// </summary>
    /// <param name="services"></param>
    /// <param name="authenticationSchemes"></param>
    public static void AddClaimAuthoriztion(this IServiceCollection services, params string[] authenticationSchemes)
    {
        if (authenticationSchemes.Length > 0)
        {
            services.Configure<ClaimAuthorizeOptions>(opt => opt.AuthenticationSchemes = authenticationSchemes);
        }

        services.AddSingleton<IAuthorizationPolicyProvider, ClaimPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, ClaimHandler>();
    }
}
#endregion

public class ClaimAuthorizeOptions
{
    public string[] AuthenticationSchemes { get; set; } = new string[] { IdentityConstants.ApplicationScheme };
}

