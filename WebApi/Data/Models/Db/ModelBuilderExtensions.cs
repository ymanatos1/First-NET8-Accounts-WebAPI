using Microsoft.EntityFrameworkCore;
using WebAPI.Data.Models;

namespace WebAPI.Data.Models.Db
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountCategory>().HasData(
                new AccountCategory { Id = 1, Name = "System Account", Description = "System accounts" },
                new AccountCategory { Id = 2, Name = "Guest Account", Description = "Guest accounts" },
                new AccountCategory { Id = 3, Name = "User Account", Description = "User accounts" },
                new AccountCategory { Id = 4, Name = "Team Account", Description = "Team accounts" },
                new AccountCategory { Id = 5, Name = "Company Account", Description = "Company accounts" },
                new AccountCategory { Id = 6, Name = "Group Account", Description = "Company/Teams Group accounts" });

            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, CategoryId = 1, Name = "Admin", Description = "System admin account", IsActive = true }); //,
                //new Account { Id = 2, CategoryId = 1, Name = "Company", IsActive = true },
                //new Account { Id = 3, CategoryId = 1, Name = "Inactive", IsActive = false },
                //new Account { Id = 4, CategoryId = 2, Name = "Guest1", IsActive = true },
                //new Account { Id = 5, CategoryId = 2, Name = "Guest2", IsActive = true },
                //new Account { Id = 6, CategoryId = 3, Name = "Company1", IsActive = true },
                //new Account { Id = 7, CategoryId = 3, Name = "Company2", IsActive = true } );
                
        }

    }
}
