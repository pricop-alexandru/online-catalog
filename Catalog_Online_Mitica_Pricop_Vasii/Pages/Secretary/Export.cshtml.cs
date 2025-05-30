using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Catalog_Online_Mitica_Pricop_Vasii.Data;

namespace OnlineCatalog.Pages.Secretary
{
    [Authorize(Roles = "Secretary")]
    public class ExportModel : PageModel
    {
        private readonly AppDbContext _context;

        public ExportModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetExportCourseGrades(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return NotFound();
            }

            using var stream = new MemoryStream();
            var document = new Document();
            PdfWriter.GetInstance(document, stream);
            document.Open();

            // Add title
            document.Add(new Paragraph($"Grade Report: {course.Title}",
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18)));
            document.Add(new Paragraph($"Academic Year: {course.AcademicYear}"));
            document.Add(new Paragraph($"Professor: {course.Professor?.FullName}"));
            document.Add(Chunk.NEWLINE);

            // Create table
            var table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.SetWidths(new[] { 3f, 1f });
            table.AddCell(new PdfPCell(new Phrase("Student Name",
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD))));
            table.AddCell(new PdfPCell(new Phrase("Grade",
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD))));

            foreach (var enrollment in course.Enrollments)
            {
                table.AddCell(enrollment.Student?.FullName ?? "Unknown");
                table.AddCell(enrollment.Grade?.ToString() ?? "-");
            }

            document.Add(table);
            document.Close();

            return File(stream.ToArray(), "application/pdf", $"{course.Title}_Grades.pdf");
        }
    }
}