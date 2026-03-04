using Microsoft.AspNetCore.Mvc;
using SistemaNomina.Data;
using SistemaNomina.Models;

namespace SistemaNomina.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search = "")
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            var depts = _context.Departments.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                depts = depts.Where(d => d.dept_name.Contains(search) || d.dept_no.Contains(search));
            ViewBag.Search = search;
            return View(depts.ToList());
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department dept)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(dept);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dept);
        }

        public IActionResult Edit(string id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            var dept = _context.Departments.Find(id);
            if (dept == null) return NotFound();
            return View(dept);
        }

        [HttpPost]
        public IActionResult Edit(Department dept)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Update(dept);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dept);
        }

        public IActionResult Delete(string id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            var dept = _context.Departments.Find(id);
            if (dept == null) return NotFound();
            return View(dept);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            var dept = _context.Departments.Find(id);
            if (dept != null)
            {
                _context.Departments.Remove(dept);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
