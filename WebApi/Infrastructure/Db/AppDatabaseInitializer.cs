using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebAPI.Data.Models.Db;

namespace WebAPI.Infrastructure.Db
{
    public class AppDatabaseInitializer
    {
        private WebApplicationBuilder _builder;

        // configuration settings
        private bool _enableInMemory;
        private bool _enableSql;
        private string _useDatabase;
        private string _sqlProvider;
        private IConfigurationSection _connStrings;

        public AppDatabaseInitializer(WebApplicationBuilder builder)
        {
            _builder = builder;

            var configuration = builder.Configuration;
            //var services = builder.Services;
            //var logging = builder.Logging;

            var accountsDbSection = configuration.GetSection("AccountsDatabase");
            _enableInMemory = accountsDbSection.GetValue<bool>("EnableInMemory");
            _enableSql = accountsDbSection.GetValue<bool>("EnableSql");
            _useDatabase = accountsDbSection["UseDatabase"] ?? "InMemory";
            _sqlProvider = accountsDbSection["SqlProvider"] ?? "Sqlite";
            _connStrings = accountsDbSection.GetSection("ConnectionStrings");
        }

        public void AddServices()
        {
            var configuration = _builder.Configuration;
            var services = _builder.Services;

            //services.AddDbContext<AccountContext>(options =>
            //{
            //    options.UseInMemoryDatabase("InMemoryAccountsDB");
            //});
            {
                // Register SqlAccountsDb
                if (_enableSql)
                {
                    services.AddDbContext<SqlAccountsContext>(options =>
                    {
                        string connStr = _connStrings[_sqlProvider]!;
                        //switch (sqlProvider)
                        //{
                        //    case "Sqlite": options.UseSqlite(connStr); break;
                        //    case "SqlServer": options.UseSqlServer(connStr); break;
                        //    case "Oracle": options.UseOracle(connStr); break;
                        //    default: throw new Exception("Unsupported SqlProvider");
                        //}
                    });
                }

                // Register InMemoryAccountsDb if enabled or fallback
                if (_enableInMemory || !_enableSql)
                {
                    services.AddDbContext<InMemoryAccountsContext>(options =>
                        options.UseInMemoryDatabase("InMemoryAccountsDb"));
                }

                // Register IAccountsDbContext
                if (!_enableSql)
                {
                    //logger.LogInformation("Using InMemoryAccountsDb via DI");
                    services.AddScoped<IAccountsDbContext, InMemoryAccountsContext>();
                }
                else if (_enableInMemory)
                {
                    services.AddScoped<IAccountsDbContext>(sp =>
                    {
                        var logger = sp.GetRequiredService<ILogger<Program>>();
                        if (_useDatabase == "InMemory")
                        {
                            logger.LogInformation("Using InMemoryAccountsDb via DI");
                            return sp.GetRequiredService<InMemoryAccountsContext>();
                        }
                        else
                        {
                            logger.LogInformation($"Using SqlAccountsDb via DI (provider: {_sqlProvider})");
                            return sp.GetRequiredService<SqlAccountsContext>();
                        }
                    });
                }
                else
                {
                    //logger.LogInformation($"Using SqlAccountsDb via DI (provider: {sqlProvider})");
                    services.AddScoped<IAccountsDbContext, SqlAccountsContext>();
                }
            }

        }

        public void ConfigureServices(WebApplication app)
        {
            {
                // Seed both databases if registered
                using (var scope = app.Services.CreateScope())
                {
                    var sp = scope.ServiceProvider;
                    var logger = sp.GetRequiredService<ILogger<Program>>();

                    if (_enableInMemory || !_enableSql)
                    {
                        logger.LogInformation("Seeding InMemoryAccountsDb...");
                        //Seed(scopedServices.GetRequiredService<InMemoryAccountsDb>());
                        sp.GetRequiredService<InMemoryAccountsContext>().Database.EnsureCreated();

                        // Make the service provider globally accessible
                        ServiceLocator.InMemoryAccountsDB = sp.GetRequiredService<InMemoryAccountsContext>();
                    }

                    if (_enableSql)
                    {
                        logger.LogInformation("Seeding SqlAccountsDb...");
                        //Seed(scopedServices.GetRequiredService<SqlAccountsDb>());
                        sp.GetRequiredService<SqlAccountsContext>().Database.EnsureCreated();

                        // Make the service provider globally accessible
                        ServiceLocator.SqlAccountsDB = sp.GetRequiredService<SqlAccountsContext>();
                    }
                }
            }
        }


    }
}