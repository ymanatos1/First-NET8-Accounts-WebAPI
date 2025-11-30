using WebAPI.Data.Models.Db;
using WebAPI.Data.Services;
using WebAPI.Infrastructure.Db;
using WebAPI.Lib.Data.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
var logging = builder.Logging;

// Add services to the container.

logging.AddConsole();
builder.Services.AddLogging();
//var logger = builder.Services.GetRequiredService<ILogger<Program>>();

//builder.Services.AddApiVersioning(options =>
//{
//    options.DefaultApiVersion = new ApiVersion(1, 0);
//    options.AssumeDefaultVersionWhenUnspecified = true;
//    options.ReportApiVersions = true;
//    options.ApiVersionReader = ApiVersionReader.Combine(
//        new QueryStringApiVersionReader("api-version"),
//        new HeaderApiVersionReader("X-API-Version"),
//        new MediaTypeApiVersionReader("ver")
//        );
//}).AddApiExplorer(options => {
//    options.GroupNameFormat = "'v'VVV";
//    options.SubstituteApiVersionInUrl = true;
//});

services.AddControllers()
    .ConfigureApiBehaviorOptions(options => {
        options.SuppressModelStateInvalidFilter = true;
    })
    .AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var databaseInitializer = new AppDatabaseInitializer(builder);
databaseInitializer.AddServices();

services.AddScoped<IAccountService, AccountService>();
services.AddScoped<IAccountCategoryService, AccountCategoryService>();

// Read the list of origins from configuration
var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();
services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policyBuilder =>
    {
        policyBuilder
            //.AllowAnyOrigin();
            .WithOrigins(allowedOrigins!) // use the list from appsettings.json)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();


//var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
//app.Urls.Add($"http://*:{port}");
//var port = Environment.GetEnvironmentVariable("PORT");
//var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
if (!string.IsNullOrEmpty(port))
{
    //builder.WebHost.UseUrls($"http://*:{port}");
    app.Urls.Add($"http://*:{port}");
    //builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

}

databaseInitializer.ConfigureServices(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHsts();  // Production only
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowSpecificOrigins");

app.MapControllers();

//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Version = "v1",
//        Title = "ToDo API",
//        Description = "An ASP.NET Core Web API for managing ToDo items",
//        TermsOfService = new Uri("https://example.com/terms"),
//        Contact = new OpenApiContact
//        {
//            Name = "Example Contact",
//            Url = new Uri("https://example.com/contact")
//        },
//        License = new OpenApiLicense
//        {
//            Name = "Example License",
//            Url = new Uri("https://example.com/license")
//        }
//    });
//});

// Make the service provider globally accessible
//ServiceLocator.Provider = app.Services;
//ServiceLocator.InMemoryAccountsDB = app.Services;

app.Run();


public static class ServiceLocator
{
    //public static IServiceProvider? Provider { get; set; }
    public static InMemoryAccountsContext? InMemoryAccountsDB { get; set; } = null;
    public static SqlAccountsContext? SqlAccountsDB { get; set; } = null;
}
