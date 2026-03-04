using Microsoft.EntityFrameworkCore;
using SistemaNomina.Data;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapGet("/seed", (ApplicationDbContext db) =>
{
    var user = db.Users.FirstOrDefault(u => u.usuario == "admin");
    if (user != null)
    {
        user.clave = BCrypt.Net.BCrypt.HashPassword("admin123");
        db.SaveChanges();
        return "Clave actualizada!";
    }
    return "Usuario no encontrado";
});
app.Run();