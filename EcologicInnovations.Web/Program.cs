using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Data.Seed;
using EcologicInnovations.Web.Services;
using EcologicInnovations.Web.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        // Email verification not required for this application
        options.SignIn.RequireConfirmedAccount = false;

        // Relax password rules: minimum length 6, no uppercase/lowercase/special required
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppPolicies.RequireAdminRole, policy =>
        policy.RequireRole(AppRoles.Admin));
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

builder.Services.AddApplicationSeeding(builder.Configuration);
builder.Services.AddEcologicInnovationsSharedServices(builder.Configuration);
builder.Services.AddEcologicInnovationsSeoInfrastructure(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error/status/{0}");

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>();
    await initializer.InitializeAsync();
}

app.Run();
