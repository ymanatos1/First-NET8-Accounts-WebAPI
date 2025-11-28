using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebAPI.Data.Models;

namespace WebAPI.Data.Models.Db;
public interface IAccountsDbContext
{
    DbSet<Account> Accounts { get; }
    DbSet<AccountCategory> AccountCategories { get; }
 
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    EntityEntry Entry(object entity);
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;


    // Access to database operations like EnsureCreated(), Migrate(), BeginTransaction(), etc.
    DatabaseFacade Database { get; }

}
