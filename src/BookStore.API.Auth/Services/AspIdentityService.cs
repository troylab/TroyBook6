using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Transactions;
using BookStore.Domain.Services;
using Microsoft.AspNetCore.Identity;

namespace BookStore.API.Auth.Services;

public class AspIdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AspIdentityService
        (
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<(bool succedded, string errors)> SetUserToRoles(string userEmail, params string[] roleNames)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
            return (false, "Cannot find the user");

        var rolesIdDb = await _userManager.GetRolesAsync(user);
        var rolesToAdd = roleNames.Except(rolesIdDb).ToList();
        var rolesToDelete = rolesIdDb.Except(roleNames).ToList();

        IdentityResult? identityResult = null;
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            if (rolesToAdd.Count > 0)
                identityResult = await _userManager.AddToRolesAsync(user, rolesToAdd);

            if (rolesToDelete.Count > 0)
                identityResult = await _userManager.RemoveFromRolesAsync(user, rolesToDelete);

            if (identityResult != null && identityResult.Succeeded)
                identityResult = await _userManager.UpdateAsync(user);

            scope.Complete();
        }

        if (identityResult == null)
            return (true, "No need to update User's roles");
        else if (!identityResult.Succeeded)
            return (false, CombineErrors(identityResult.Errors));

        return (true, "");
    }

    public async Task<(bool succedded, string errors)> CreateIdentityRole(string roleName, params Claim[] claims)
    {
        if (await _roleManager.FindByNameAsync(roleName) != null)
            return (false, $"The role '{roleName}' already exists.");

        var role = new IdentityRole { Name = roleName };

        var r = await _roleManager.CreateAsync(role);
        if (r.Succeeded)
        {
            foreach (var c in claims)
            {
                await _roleManager.AddClaimAsync(role, c);
            }

            return (true, "");
        }
        else
        {
            return (false, CombineErrors(r.Errors));
        }
    }

    public async Task<(bool succedded, string errors, string idenityUserId)> CreateIdentityUser(string email, string password, bool isAdmin)
    {
        var u = new IdentityUser
        {
            UserName = email,
            Email = email,
        };

        var r = await _userManager.CreateAsync(u, password);
        if (!r.Succeeded)
            return (false, CombineErrors(r.Errors), "");

        if (isAdmin)
            await _userManager.AddClaimAsync(u, new Claim("IsAdmin", "true"));

        return (true, "", u.Id);
    }

    public async Task<(bool succedded, string errors)> SetRoleClaims(string roleName, params Claim[] claims)
    {
        var role = await _roleManager.FindByNameAsync(roleName);

        foreach (var c in claims)
        {
            var r = await _roleManager.AddClaimAsync(role, c);
            if (!r.Succeeded)
                return (false, CombineErrors(r.Errors));
        }

        return (true, "");
    }

    public async Task<(bool succedded, string errors)> SetUserAsAdmin(string email)
    {
        var u = await _userManager.FindByEmailAsync(email);
        if (u == null)
            return (false, "The user doesn't exists.");

        var r = await _userManager.AddClaimAsync(u, new Claim("IsAdmin", "true"));
        if (r.Succeeded)
            return (true, "");
        else
            return (false, CombineErrors(r.Errors));
    }

    public async Task<(bool succedded, string errors)> RemoveAdmin(string email)
    {
        var u = await _userManager.FindByEmailAsync(email);
        if (u == null)
            return (false, "The user doesn't exists.");

        var r = await _userManager.RemoveClaimAsync(u, new Claim("IsAdmin", "true"));
        if (r.Succeeded)
            return (true, "");
        else
            return (false, CombineErrors(r.Errors));
    }

    public async Task<(bool succedded, string errors)> SetUserClaims(string email, params Claim[] claims)
    {
        var u = await _userManager.FindByEmailAsync(email);
        if (u == null)
            return (false, "The user doesn't exists.");

        var r = await _userManager.AddClaimsAsync(u, claims);
        if (r.Succeeded)
            return (true, "");
        else
            return (false, CombineErrors(r.Errors));
    }

    public async Task<IList<Claim>?> GetUserClaims(string email)
    {
        var u = await _userManager.FindByEmailAsync(email);
        if (u == null)
            return null;

        var claims = await _userManager.GetClaimsAsync(u);
        return claims;
    }

    private string CombineErrors(IEnumerable<IdentityError> identityErrors)
    {
        if (identityErrors.Any())
            return string.Join("\n", identityErrors.Select(t => t.Description));
        else
            return "";
    }
}

