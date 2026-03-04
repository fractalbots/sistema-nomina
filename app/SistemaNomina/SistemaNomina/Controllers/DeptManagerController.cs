using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaNomina.Data;
using SistemaNomina.Models;

namespace SistemaNomina.Controllers
{
    public class DeptManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DeptManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            var list = _context.DeptManagers
                .Include(d => d.Employee)
                .Include(d => d.Department)
                .ToList();
            return View(list);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(DeptManager dm)
        {
            // Validar un solo manager activo por departamento
            bool existe = _context.DeptManagers.Any(x => x.dept_no == dm.dept_no && x.to_date == null);
            if (existe)
                ModelState.AddModelError("", "Ya existe un gerente activo para este departamento.");

            if (ModelState.IsValid)
            {
                _context.DeptManagers.Add(dm);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            return View(dm);
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            var dm = _context.DeptManagers.Find(id);
            if (dm == null) return NotFound();
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            return View(dm);
        }

        [HttpPost]
        public IActionResult Edit(DeptManager dm)
        {
            if (ModelState.IsValid)
            {
                _context.DeptManagers.Update(dm);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            return View(dm);
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            var dm = _context.DeptManagers
                .Include(d => d.Employee)
                .Include(d => d.Department)
                .FirstOrDefault(d => d.Id == id);
            if (dm == null) return NotFound();
            return View(dm);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var dm = _context.DeptManagers.Find(id);
            if (dm != null)
            {
                _context.DeptManagers.Remove(dm);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
