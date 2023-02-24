using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.API.Auth.Controllers;

[Route("api/[controller]")]
public class AuthController : Controller
{

    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController
        (
            UserManager<IdentityUser> userManager,
            IConfiguration configuration
        )
    {
        _userManager = userManager;
        _configuration = configuration;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginInfo.Rq rq)
    {
        var user = await _userManager.FindByEmailAsync(rq.Email);
        if (user == null || !(await _userManager.CheckPasswordAsync(user, rq.Password)))
            throw new Exception("user or password incorrect");

        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var userClaims = await _userManager.GetClaimsAsync(user);
        foreach (var claim in userClaims)
        {
            authClaims.Add(new Claim(claim.Type, claim.Value));
        }

        var token = GetToken(authClaims);

        var rs = new LoginInfo.Rs
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        };

        return new OkObjectResult(rs);
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }

    public class LoginInfo
    {
        public class Rq
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
        }

        public class Rs
        {
            [JsonPropertyName("token")]
            public string Token { get; set; } = default!;
            [JsonPropertyName("expiration")]
            public DateTime Expiration { get; set; } = default!;
        }
    }
}

