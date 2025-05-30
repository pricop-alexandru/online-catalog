using Catalog_Online_Mitica_Pricop_Vasii.Data;
using Catalog_Online_Mitica_Pricop_Vasii.Models;
using Catalog_Online_Mitica_Pricop_Vasii.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Catalog_Online_Mitica_Pricop_Vasii.Pages.Professor
{
    [Authorize(Roles = "Professor")]
    public class GradeBookModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly NotificationService _notificationService;

        public GradeBookModel(AppDbContext context, NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        [BindProperty]
        public List<Enrollment> Enrollments { get; set; } = new();

        public Course Course { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Course = await _context.Courses
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.Id == courseId && c.ProfessorId == userId);

            if (Course == null)
            {
                return NotFound();
            }

            Enrollments = Course.Enrollments.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            foreach (var enrollment in Enrollments)
            {
                var dbEnrollment = await _context.Enrollments.FindAsync(enrollment.Id);
                if (dbEnrollment != null && dbEnrollment.Grade != enrollment.Grade)
                {
                    dbEnrollment.Grade = enrollment.Grade;
                    await _notificationService.NotifyGradeAddedAsync(dbEnrollment.Id);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}