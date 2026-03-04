using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaNomina.Data;
using SistemaNomina.Models;

namespace SistemaNomina.Controllers
{
    public class DeptEmpController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DeptEmpController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search = "")
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");

            var list = _context.DeptEmps
                .Include(d => d.Employee)
                .Include(d => d.Department)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                list = list.Where(d => d.Employee.first_name.Contains(search) ||
                                       d.Employee.last_name.Contains(search) ||
                                       d.dept_no.Contains(search));
            ViewBag.Search = search;
            return View(list.ToList());
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
        public IActionResult Create(DeptEmp de)
        {
            // Validar solapamiento
            bool overlap = _context.DeptEmps.Any(x => x.emp_no == de.emp_no &&
                x.to_date == null || (x.emp_no == de.emp_no && string.Compare(x.to_date, de.from_date) > 0));

            if (overlap)
                ModelState.AddModelError("", "El empleado ya tiene una asignación activa en ese período.");

            if (ModelState.IsValid)
            {
                _context.DeptEmps.Add(de);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            return View(de);
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            var de = _context.DeptEmps.Find(id);
            if (de == null) return NotFound();
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            return View(de);
        }

        [HttpPost]
        public IActionResult Edit(DeptEmp de)
        {
            if (ModelState.IsValid)
            {
                _context.DeptEmps.Update(de);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            return View(de);
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            var de = _context.DeptEmps
                .Include(d => d.Employee)
                .Include(d => d.Department)
                .FirstOrDefault(d => d.Id == id);
            if (de == null) return NotFound();
            return View(de);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var de = _context.DeptEmps.Find(id);
            if (de != null)
            {
                _context.DeptEmps.Remove(de);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
