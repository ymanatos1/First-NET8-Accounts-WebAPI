using WebAPI.Lib.Data.Services;
using WebApp.Lib.Data.Services;
using WebApp.Lib.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------
// SERVICES
// ----------------------------------------------------------

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// MVC Views + Controllers
builder.Services.AddControllersWithViews();   // MVC

//builder.Services.AddControllersWithViews()
//    .AddApplicationPart(typeof(WebApp.Razor.UI.Lib.ViewComponents.AccountListTableViewComponent).Assembly)
//    .AddRazorRuntimeCompilation();

// Razor Pages (optional – only if you have *.cshtml Razor Pages)
//builder.Services.AddRazorPages();

// HealthChecks
builder.Services.AddHealthChecks()
    .AddCheck<MyRandomHealthCheck>("MyRandomHealthCheck")
    .AddCheck<ApiAliveHealthCheck>("ApiAliveHealthCheck");

// HttpClient for REST API calls
builder.Services.AddHttpClient();

// Application services
builder.Services.AddTransient<IAccountServiceClient, AccountServiceClient>();
builder.Services.AddTransient<IAccountCategoryServiceClient, AccountCategoryServiceClient>();


//// Add services to the container.

//builder.Logging.AddConsole();
//builder.Services.AddLogging();
////var logger = builder.Services.GetRequiredService<ILogger<Program>>();

//builder.Services.AddControllersWithViews();   // MVC + Razor views
//builder.Services.AddRazorPages();             // (Optional) if you also serve Razor Pages

////builder.Services.AddHealthChecks()
////    .AddCheck("My random health check",
////              () => (new Random().Next() % 2 == 0) ? HealthCheckResult.Healthy("Good") : HealthCheckResult.Degraded("Not so good"));
//builder.Services.AddHealthChecks()
//    .AddCheck<MyRandomHealthCheck>("My random health check")
//    .AddCheck<ApiAliveHealthCheck>("Api alive health check");

//// registers HttpClient using DI
//builder.Services.AddHttpClient();

//builder.Services.AddTransient<IAccountServiceClient, AccountServiceClient>();

var app = builder.Build();


// ----------------------------------------------------------
// MIDDLEWARE PIPELINE
// ----------------------------------------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Health Check endpoint
app.MapHealthChecks("/health");

// MVC default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Razor Pages route (only if you use Razor Pages)
//app.MapRazorPages();


//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

////app.MapRazorPages();
////app.MapRazorComponents<Component1>();
//////       .AddInteractiveServerRenderMode();
////app.MapRazorComponents<AccountsListTableComponent>();
//////.AddInteractiveServerRenderMode();

//// app.MapControllers();      // Only if you use API controllers
////app.MapDefaultControllerRoute();   // Required for MVC controllers + views
////app.MapRazorPages();               // Only if you use Razor Pages also

//app.MapHealthChecks("/health");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();               // Only if you use Razor Pages also

app.Run();
