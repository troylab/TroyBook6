using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Domain.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.API.Auth.Controllers;

[ClaimAuthorize(SysClaims.PermissionManage)]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IIdentityService _identityService;


    public UserController
        (
            IIdentityService identityService
        )
    {
        _identityService = identityService;
    }
}

