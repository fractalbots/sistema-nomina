using Microsoft.EntityFrameworkCore;
using SistemaNomina.Models;
using static System.Net.WebRequestMethods;

namespace SistemaNomina.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DeptEmp> DeptEmps { get; set; }
        public DbSet<DeptManager> DeptManagers { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<LogAuditoriaSalarios> LogAuditoriaSalarios { get; set; }
    }
}