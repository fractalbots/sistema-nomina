using Microsoft.AspNetCore.Mvc;
using SistemaNomina.Data;
using SistemaNomina.Models;
using BCrypt.Net;

namespace SistemaNomina.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string usuario, string clave)
        {
            var user = _context.Users.FirstOrDefault(u => u.usuario == usuario);
            if (user != null && BCrypt.Net.BCrypt.Verify(clave, user.clave))
            {
                HttpContext.Session.SetString("Usuario", user.usuario);
                HttpContext.Session.SetString("Rol", user.rol);
                HttpContext.Session.SetInt32("EmpNo", user.emp_no);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Usuario o clave incorrectos";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}