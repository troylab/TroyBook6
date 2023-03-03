using System;
using System.Security.Claims;

namespace BookStore.Domain.Services;

public interface IIdentityService
{
    Task<(bool succedded, string errors, string idenityUserId)> CreateIdentityUser(string email, string password, bool isAdmin);
    Task<(bool succedded, string errors)> CreateIdentityRole(string roleName, params Claim[] claims);
    Task<(bool succedded, string errors)> SetUserToRoles(string userEmail, params string[] roleNames);
    Task<(bool succedded, string errors)> SetUserAsAdmin(string email);
    Task<(bool succedded, string errors)> RemoveAdmin(string email);
    Task<(bool succedded, string errors)> SetUserClaims(string email, params Claim[] claims);
    Task<(bool succedded, string errors)> SetRoleClaims(string roleName, params Claim[] claims);
    Task<IList<Claim>?> GetUserClaims(string email);
}

