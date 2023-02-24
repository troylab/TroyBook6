using System.Security.Claims;
using System.Transactions;
using BookStore.API.Auth;
using BookStore.Repository.EFCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add dbcontext
builder.Services.AddDbContext<BookStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
    options.UseLoggerFactory(new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() }));
});

builder.Services.AddIdentityCore<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    options.User.AllowedUserNameCharacters = null;

    // 設定User密碼複雜度
    options.Password.RequireNonAlphanumeric = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<BookStoreDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddEFRepositories();
builder.Services.AddDomainServices();
builder.Services.AddDomainManagers();

builder.Services.AddControllers();

/*加入JWT驗證機制*/
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

/*加入 ClaimAuthorization 權限控管*/
builder.Services.AddClaimAuthoriztion(JwtBearerDefaults.AuthenticationScheme);

var app = builder.Build();

//建立 ideneity 假資料
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var adminEmail = "admin@test.com";
    var managerEmail = "manager@test.com";
    var aliceEmail = "alice@test.com";

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
        };
        //建立 admin user, 密碼 Aa123456
        await userManager.CreateAsync(adminUser, "Aa123456");
        await userManager.AddClaimAsync(adminUser, new Claim("IsAdmin", "true"));

        var managerUer = new IdentityUser
        {
            UserName = managerEmail,
            Email = managerEmail
        };
        //建立 Manager 角色
        var managerRole = new IdentityRole { Name = "Manager" };
        await roleManager.CreateAsync(managerRole);
        await roleManager.AddClaimAsync(managerRole, new Claim(SysClaims.BookManage, ""));

        await userManager.CreateAsync(managerUer, "Aa123456");
        await userManager.AddToRoleAsync(managerUer, managerRole.Name);

        // 建立普通user
        var alice = new IdentityUser
        {
            UserName = aliceEmail,
            Email = aliceEmail
        };
        await userManager.CreateAsync(alice, "Aa123456");
    }
}


// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

