using BookStore.Repository.EFCore;
using BookStore.API;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using System.Text.Encodings.Web;

var isProd = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production;
var logger = NLog.LogManager.Setup().LoadConfigurationFromFile(isProd ? "nlog.config" : "nlog.dev.config").GetCurrentClassLogger();
logger.Debug($"init main: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // NLog
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    // Add dbcontext
    builder.Services.AddDbContext<BookStoreDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
        options.UseLoggerFactory(new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() }));
    });

    /*從 appsettings.json 讀取 DomainOptions 設定值*/
    //builder.Services.ConfigureDomainOptions<DomainOptions>(builder.Configuration.GetSection("DomainOptions"));
    builder.Services.ConfigureDomainOptions<DomainOptions>(builder.Configuration);

    // Add services to the container.
    builder.Services.AddEFRepositories();
    builder.Services.AddDomainServices();
    builder.Services.AddDomainManagers();

    builder.Services.AddControllers().AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All);
        opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

public partial class Program { }
