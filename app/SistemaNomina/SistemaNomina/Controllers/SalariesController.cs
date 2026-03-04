using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaNomina.Data;
using SistemaNomina.Models;

namespace SistemaNomina.Controllers
{
    public class SalariesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SalariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search = "")
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");

            var list = _context.Salaries.Include(s => s.Employee).AsQueryable();
            if (!string.IsNullOrEmpty(search))
                list = list.Where(s => s.Employee.first_name.Contains(search) ||
                                       s.Employee.last_name.Contains(search));
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
        public IActionResult Create(Salary sal)
        {
            // Cerrar salario anterior activo
            var anterior = _context.Salaries.FirstOrDefault(s => s.emp_no == sal.emp_no && s.to_date == null);
            if (anterior != null)
                anterior.to_date = sal.from_date;

            if (ModelState.IsValid)
            {
                _context.Salaries.Add(sal);

                // Auditoría
                _context.LogAuditoriaSalarios.Add(new LogAuditoriaSalarios
                {
                    usuario = HttpContext.Session.GetString("Usuario") ?? "sistema",
                    fechaActualizacion = DateTime.Now,
                    DetalleCambio = "Nuevo salario registrado",
                    salario = sal.salary,
                    emp_no = sal.emp_no
                });

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employees = _context.Employees.ToList();
            return View(sal);
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            var sal = _context.Salaries.Find(id);
            if (sal == null) return NotFound();
            ViewBag.Employees = _context.Employees.ToList();
            return View(sal);
        }

        [HttpPost]
        public IActionResult Edit(Salary sal)
        {
            if (ModelState.IsValid)
            {
                _context.Salaries.Update(sal);

                // Auditoría
                _context.LogAuditoriaSalarios.Add(new LogAuditoriaSalarios
                {
                    usuario = HttpContext.Session.GetString("Usuario") ?? "sistema",
                    fechaActualizacion = DateTime.Now,
                    DetalleCambio = "Salario modificado",
                    salario = sal.salary,
                    emp_no = sal.emp_no
                });

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employees = _context.Employees.ToList();
            return View(sal);
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            var sal = _context.Salaries.Include(s => s.Employee).FirstOrDefault(s => s.Id == id);
            if (sal == null) return NotFound();
            return View(sal);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var sal = _context.Salaries.Find(id);
            if (sal != null)
            {
                _context.Salaries.Remove(sal);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Auditoria()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            var logs = _context.LogAuditoriaSalarios.OrderByDescending(l => l.fechaActualizacion).ToList();
            return View(logs);
        }
    }
}
