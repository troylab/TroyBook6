using System;
using BookStore.Repository.EFCore;
using BookStore.Domain.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookStore.API.Test;

public class PlaygroundApplication : WebApplicationFactory<Program>
{
    public PlaygroundApplication()
    {
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        string connectionString = "";
        if (File.Exists("appsettings.Testing.json"))
        {
            var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.Testing.json").Build();
            connectionString = configuration.GetConnectionString("Default");
        }
        else
        {
            //for azure CI docker sqlserver
            connectionString = "Server=127.0.0.1;Database=BookStore;User Id=sa;Password=Aa123456;MultipleActiveResultSets=true";
        }

        // Add mock/test services to the builder here
        builder.ConfigureServices(services =>
        {
            /* remove the DbContext added in Program.cs*/
            /* ref. https://stackoverflow.com/questions/67876503/replace-dbcontext-in-webapplicationfactory-for-unit-testing */
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BookStoreDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            /* use testing sql server */
            services.AddDbContext<BookStoreDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
                options.UseLoggerFactory(new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() }));
            }, ServiceLifetime.Scoped);

            //在這邊替換 services
            services.AddSingleton<IImageStorage, FakeImageStorage>();

            services.AddEFRepositories();
        });

        return base.CreateHost(builder);
    }
}

