using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebAPI.Data.Models;

namespace WebAPI.Data.Models.Db
{
    public class InMemoryAccountsContext : DbContext, IAccountsDbContext
    {
        public InMemoryAccountsContext(DbContextOptions<InMemoryAccountsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountCategory>()
                .HasMany(c => c.Accounts)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId);

            modelBuilder.Seed();
        }

        //public DbSet<Account> Accounts { get; set; }
        //public DbSet<AccountCategory> Categories { get; set; }


        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<AccountCategory> AccountCategories { get; set; }

        public new int SaveChanges() => base.SaveChanges();

        public new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);

        public new EntityEntry Entry(object entity) => base.Entry(entity);

        public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
            => base.Entry(entity);


        public new DatabaseFacade Database => base.Database;

    }


    public class SqlAccountsContext : DbContext, IAccountsDbContext
    {
        public SqlAccountsContext(DbContextOptions<SqlAccountsContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountCategory>()
                .HasMany(c => c.Accounts)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId);

            modelBuilder.Seed();
        }

        //public DbSet<Account> Accounts { get; set; }
        //public DbSet<AccountCategory> Categories { get; set; }


        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<AccountCategory> AccountCategories { get; set; }

        public new int SaveChanges() => base.SaveChanges();

        public new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);

        public new EntityEntry Entry(object entity) => base.Entry(entity);

        public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
            => base.Entry(entity);


        public new DatabaseFacade Database => base.Database;

    }


}
