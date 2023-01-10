using System;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace BookStore.Repository.EFCore;

public class EFIntegrationBase
{
    protected readonly BookStoreDbContext _dbContext;

    protected DbConnection _conn => _dbContext.Database.GetDbConnection();


    public EFIntegrationBase(BookStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}

