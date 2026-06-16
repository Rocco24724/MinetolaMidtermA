using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MinetolaMidtermA.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    var adminRole = "Admin";
    if (!roleManager.RoleExistsAsync(adminRole).GetAwaiter().GetResult())
    {
        roleManager.CreateAsync(new IdentityRole(adminRole)).GetAwaiter().GetResult();
    }

    var adminEmail = "examadmin@gc.ca";
    var admin = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
    if (admin == null)
    {
        admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var createResult = userManager.CreateAsync(admin, "Test123$").GetAwaiter().GetResult();
        if (createResult.Succeeded)
        {
            userManager.AddToRoleAsync(admin, adminRole).GetAwaiter().GetResult();
        }
    }
}

app.Run();