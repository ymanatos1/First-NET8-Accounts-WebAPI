using WebAPI.Lib.Data.Services;
using WebApp.Lib.Data.Services;
using WebApp.Lib.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.AddConsole();
builder.Services.AddLogging();
//var logger = builder.Services.GetRequiredService<ILogger<Program>>();

//builder.Services.AddHealthChecks()
//    .AddCheck("My random health check",
//              () => (new Random().Next() % 2 == 0) ? HealthCheckResult.Healthy("Good") : HealthCheckResult.Degraded("Not so good"));
builder.Services.AddHealthChecks()
    .AddCheck<MyRandomHealthCheck>("My random health check")
    .AddCheck<ApiAliveHealthCheck>("Api alive health check");

// registers HttpClient using DI
builder.Services.AddHttpClient();

builder.Services.AddTransient<IAccountServiceClient, AccountServiceClient>();
builder.Services.AddTransient<IAccountCategoryServiceClient, AccountCategoryServiceClient>();

// Add services to the container.
builder.Services.AddRazorPages();

//builder.Services.AddControllersWithViews();

// Tell ASP.NET to search the referenced library for Razor files
//builder.Services.AddMvc()
//    .AddApplicationPart(typeof(WebApp.Razor.UI.Lib.ViewComponents.AccountListTableViewComponent).Assembly);
//.AddRazorRuntimeCompilation();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapRazorPages();
////app.MapRazorComponents<Component1>();
//////       .AddInteractiveServerRenderMode();
////app.MapRazorComponents<AccountsListTableComponent>();
////       //.AddInteractiveServerRenderMode();
//app.MapDefaultControllerRoute();

app.MapRazorPages();
app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
