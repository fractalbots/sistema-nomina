using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaNomina.Data;
using SistemaNomina.Models;

namespace SistemaNomina.Controllers
{
    public class TitlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TitlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search = "")
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");

            var list = _context.Titles.Include(t => t.Employee).AsQueryable();
            if (!string.IsNullOrEmpty(search))
                list = list.Where(t => t.Employee.first_name.Contains(search) ||
                                       t.Employee.last_name.Contains(search) ||
                                       t.title.Contains(search));
            ViewBag.Search = search;
            return View(list.ToList());
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            ViewBag.Employees = _context.Employees.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Title t)
        {
            if (ModelState.IsValid)
            {
                _context.Titles.Add(t);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employees = _context.Employees.ToList();
            return View(t);
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            var t = _context.Titles.Find(id);
            if (t == null) return NotFound();
            ViewBag.Employees = _context.Employees.ToList();
            return View(t);
        }

        [HttpPost]
        public IActionResult Edit(Title t)
        {
            if (ModelState.IsValid)
            {
                _context.Titles.Update(t);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employees = _context.Employees.ToList();
            return View(t);
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            var t = _context.Titles.Include(x => x.Employee).FirstOrDefault(x => x.Id == id);
            if (t == null) return NotFound();
            return View(t);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var t = _context.Titles.Find(id);
            if (t != null)
            {
                _context.Titles.Remove(t);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
