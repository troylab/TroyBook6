using System;
using EFCore.BulkExtensions;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Security.Principal;
using BookStore.Repository.EFCore;
using Microsoft.Extensions.Logging;
using BookStore.Repository.EFCore.Repositories;

namespace BookStore.Repository.Test;

// 所有的 Repository test classes 都要標注 [Collection("Database collection")
// 因為預設 xunit 的多執行緒執行所有 tests ， 建立 db 只要執行一次
[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { /*for not parallel run all repository testing*/ }

public class DatabaseFixture : IDisposable
{
    public ServiceProvider Sp { get; private set; }

    public DatabaseFixture()
    {
        var services = new ServiceCollection();

        services.AddScoped<IBookStoreRepository, EFBookStoreRepository>();

        UseLocalSqlServer(services);

        Sp = services.BuildServiceProvider();

        PrepareNewLocalSqlServer();
    }

    public void Dispose()
    {
    }


    private void UseLocalSqlServer(ServiceCollection services)
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


        services.AddDbContext<BookStoreDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.UseLoggerFactory(new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() }));

        }, ServiceLifetime.Scoped);
    }

    private void PrepareNewLocalSqlServer()
    {
        if (File.Exists("_dbinit"))
            return;

        var db = Sp.GetService<BookStoreDbContext>()!;
        //db.Database.EnsureDeleted();
        db.Database.ExecuteSqlRaw(DROP_ALL_TABLES);
        db.Database.EnsureCreated();

        InsertData(db.Books, GetData<Book>("Book.json"));
        InsertData(db.BookImages, GetData<BookImage>("BookImage.json"));


        File.WriteAllText("_dbinit", "");


        List<T> GetData<T>(string filename)
        {
            var filePath = Path.Combine("TestJsonData", filename);
            return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(filePath)) ?? throw new Exception($"load {filename} failed");
        }

        void InsertData<T>(DbSet<T> dbSet, List<T> data) where T : class
        {
            var bulkconfig = new BulkConfig { BatchSize = 50000, BulkCopyTimeout = 1500, SqlBulkCopyOptions = Microsoft.Data.SqlClient.SqlBulkCopyOptions.KeepIdentity };
            var tablename = typeof(T).Name;
            using var trans = db.Database.BeginTransaction();
            db.BulkInsert(data, bulkconfig);
            trans.Commit();
        }
    }

    const string DROP_ALL_TABLES = @"
-- drop constraints
DECLARE @DropConstraints NVARCHAR(max) = ''
SELECT @DropConstraints += 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.'
                        +  QUOTENAME(OBJECT_NAME(parent_object_id)) + ' ' + 'DROP CONSTRAINT' + QUOTENAME(name)
FROM sys.foreign_keys
EXECUTE sp_executesql @DropConstraints;

-- drop tables
DECLARE @DropTables NVARCHAR(max) = ''
SELECT @DropTables += 'DROP TABLE ' + QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
EXECUTE sp_executesql @DropTables;
";
}
