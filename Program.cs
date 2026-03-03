using CornHoleRevamp.Data;
using CornHoleRevamp.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyRULES", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddScoped<RetrievalUser>();
builder.Services.AddScoped<PasswordServices>();

builder.Services.AddSession(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // This creates/updates tables automatically
        await context.Database.MigrateAsync();
        Console.WriteLine("✅ Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Migration error: {ex.Message}");
        Console.WriteLine($"❌ Stack trace: {ex.StackTrace}");
        // Re-throw to crash the app so you know migration failed
        throw;
    }
}
// 🔥 END AUTO-MIGRATION 🔥

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseCors("MyRULES");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();