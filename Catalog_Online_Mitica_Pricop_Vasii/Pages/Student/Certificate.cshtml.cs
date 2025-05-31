using Catalog_Online_Mitica_Pricop_Vasii.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Catalog_Online_Mitica_Pricop_Vasii.Models;
using System.Security.Claims;

namespace Catalog_Online_Mitica_Pricop_Vasii.Pages.Student
{
    [Authorize(Roles = "Student")]
    public class CertificateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CertificateModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetGenerateCertificateAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = await _context.Users.FindAsync(userId);

            if (student == null)
            {
                return NotFound();
            }

            // Check enrollment in current academic year
            var currentYear = DateTime.Now.Year;
            var nextYear = currentYear + 1;
            var academicYear = $"{currentYear}-{nextYear}";

            var isEnrolled = await _context.Enrollments
                .AnyAsync(e => e.StudentId == userId &&
                               e.Course.AcademicYear == academicYear);

            if (!isEnrolled)
            {
                return BadRequest("Not enrolled in current academic year");
            }

            // Generate PDF
            using var stream = new MemoryStream();
            var document = new Document();
            PdfWriter.GetInstance(document, stream);
            document.Open();

            // Add content
            document.Add(new Paragraph("STUDENT CERTIFICATE",
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24)));
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(new Paragraph($"This is to certify that",
                FontFactory.GetFont(FontFactory.HELVETICA, 16)));
            document.Add(new Paragraph($"{student.FullName}",
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20)));
            document.Add(new Paragraph($"is a duly registered student at our university",
                FontFactory.GetFont(FontFactory.HELVETICA, 16)));
            document.Add(new Paragraph($"for the academic year {academicYear}.",
                FontFactory.GetFont(FontFactory.HELVETICA, 16)));
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(new Paragraph($"Issued on: {DateTime.Now.ToShortDateString()}"));
            document.Add(Chunk.NEWLINE);
            document.Add(new Paragraph("University Seal"));

            document.Close();

            return File(stream.ToArray(), "application/pdf", "Student_Certificate.pdf");
        }
    }
}