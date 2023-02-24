using System;
using BookStore.Domain;
using BookStore.Domain.Repositories;
using BookStore.Domain.Services;
using BookStore.Repository.EFCore;
using BookStore.Repository.EFCore.Repositories;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace BookStore.API.Auth;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Strongly typed configuration that allow injection of any class without IOptions
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static TOptions ConfigureDomainOptions<TOptions>(this IServiceCollection services, IConfiguration configuration) where TOptions : class, new()
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        var config = new TOptions();
        configuration.Bind(config);
        services.AddSingleton(config);
        return config;
    }

    // <summary>
    /// Strongly typed configuration that allow injection of any class without IOptions
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="services"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureDomainOptions<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions) where TOptions : class, new()
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

        if (Activator.CreateInstance(typeof(TOptions)) is TOptions options)
        {
            configureOptions(options);
            services.AddSingleton(options);
            return services;
        }
        else
        {
            throw new Exception($"{typeof(TOptions).FullName} is ivalid!");
        }
        //var options = (TOptions)Activator.CreateInstance(typeof(TOptions));
        //configureOptions(options);
        //services.AddSingleton(options);
        //return services;
    }

    /// <summary>
    /// 設定 Domain 有關的 services register
    /// </summary>
    /// <param name="services"></param>
    public static void AddDomainServices(this IServiceCollection services)
    {
        //services.AddScoped<IImageStorage, LocalDiskImageStorage>();
        //services.AddScoped<IImageProcessor, FakeImageProcessor>();
    }

    public static void AddDomainManagers(this IServiceCollection services)
    {
        //services.AddScoped<BookManager>();
    }

    public static void AddEFRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, EFBookRepository>();
        services.AddScoped<IBookImageRepository, EFBookImageRepository>();
        services.AddScoped<IBookStoreRepository, EFBookStoreRepository>();
    }
}

