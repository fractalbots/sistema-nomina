using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaNomina.Data;

namespace SistemaNomina.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");
            return View();
        }

        public IActionResult NominaVigente()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");

            var data = _context.Salaries
                .Include(s => s.Employee)
                .Where(s => s.to_date == null)
                .ToList();

            using var ms = new MemoryStream();
            var doc = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            doc.Add(new Paragraph("Nómina Vigente", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD)));
            doc.Add(new Paragraph($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}"));
            doc.Add(Chunk.NEWLINE);

            var table = new PdfPTable(4);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 3, 3, 2, 2 });

            // Encabezados
            foreach (var h in new[] { "Nombre", "Correo", "Salario", "Desde" })
            {
                var cell = new PdfPCell(new Phrase(h, new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD)));
                cell.BackgroundColor = BaseColor.DARK_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 5;
                var f = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE);
                cell.Phrase = new Phrase(h, f);
                table.AddCell(cell);
            }

            foreach (var s in data)
            {
                table.AddCell($"{s.Employee?.first_name} {s.Employee?.last_name}");
                table.AddCell(s.Employee?.correo ?? "");
                table.AddCell($"${s.salary:N0}");
                table.AddCell(s.from_date);
            }

            doc.Add(table);
            doc.Close();

            return File(ms.ToArray(), "application/pdf", "NominaVigente.pdf");
        }

        public IActionResult CambiosSalariales(string desde = "", string hasta = "")
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Auth");

            var logs = _context.LogAuditoriaSalarios.AsQueryable();

            if (!string.IsNullOrEmpty(desde))
                logs = logs.Where(l => l.fechaActualizacion >= DateTime.Parse(desde));
            if (!string.IsNullOrEmpty(hasta))
                logs = logs.Where(l => l.fechaActualizacion <= DateTime.Parse(hasta));

            ViewBag.Desde = desde;
            ViewBag.Hasta = hasta;
            return View(logs.OrderByDescending(l => l.fechaActualizacion).ToList());
        }

        public IActionResult ExportarCambiosPDF(string desde = "", string hasta = "")
        {
            var logs = _context.LogAuditoriaSalarios.AsQueryable();
            if (!string.IsNullOrEmpty(desde))
                logs = logs.Where(l => l.fechaActualizacion >= DateTime.Parse(desde));
            if (!string.IsNullOrEmpty(hasta))
                logs = logs.Where(l => l.fechaActualizacion <= DateTime.Parse(hasta));

            using var ms = new MemoryStream();
            var doc = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            doc.Add(new Paragraph("Cambios Salariales", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD)));
            doc.Add(new Paragraph($"Período: {desde} - {hasta}"));
            doc.Add(Chunk.NEWLINE);

            var table = new PdfPTable(4);
            table.WidthPercentage = 100;

            foreach (var h in new[] { "Usuario", "Fecha", "Detalle", "Salario" })
            {
                var cell = new PdfPCell();
                cell.BackgroundColor = BaseColor.DARK_GRAY;
                cell.Padding = 5;
                cell.Phrase = new Phrase(h, new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.WHITE));
                table.AddCell(cell);
            }

            foreach (var l in logs)
            {
                table.AddCell(l.usuario);
                table.AddCell(l.fechaActualizacion.ToString("dd/MM/yyyy HH:mm"));
                table.AddCell(l.DetalleCambio);
                table.AddCell($"${l.salario:N0}");
            }

            doc.Add(table);
            doc.Close();

            return File(ms.ToArray(), "application/pdf", "CambiosSalariales.pdf");
        }
    }
}
