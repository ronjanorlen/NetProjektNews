using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetProjektNews.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
.AddRoles<IdentityRole>() // Aktivera användarroller 
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

// Skapa roller och användare 
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(); // Initiera rolemanager 

    // Skapa roller 
    var roles = new[] { "Admin", "Editor" }; // Array med roller (Admin = full behörighet, Editor = kan skriva/hantera inlägg & kategorier, övriga = kan se medlemssida endast)
    // Loopa igenom rollerna 
    foreach (var role in roles)
    {
        // Kontrollera om roll redan finns
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Skapa användare 
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>(); // Initiera usermanager 
    // array med användare 
    var users = new[] {
        new { Email = "admin@test.com", Password = "Password123!", Role = "Admin" },
        new { Email = "editor@test.com", Password = "Password123!", Role = "Editor" }
    };

    // Loopa igenom
    foreach (var user in users) {
        var identityUser = await userManager.FindByEmailAsync(user.Email); // hitta användare baserat på epost
        // om användare inte finns, skapa ny
        if (identityUser == null) {
            identityUser = new IdentityUser { UserName = user.Email, Email = user.Email };
            // lagra i databas 
            await userManager.CreateAsync(identityUser, user.Password);

            // Tilldela användare en roll 
            await userManager.AddToRoleAsync(identityUser, user.Role);
        }
    }
}

app.Run();
