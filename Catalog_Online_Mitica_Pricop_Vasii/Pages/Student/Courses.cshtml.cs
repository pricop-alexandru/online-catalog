using Catalog_Online_Mitica_Pricop_Vasii.Data;
using Catalog_Online_Mitica_Pricop_Vasii.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Catalog_Online_Mitica_Pricop_Vasii.Pages.Student
{
    [Authorize(Roles = "Student")]
    public class CoursesModel : PageModel
    {
        private readonly AppDbContext _context;

        public CoursesModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Enrollment> Enrollments { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; } = "name";

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _context.Enrollments
                .Include(e => e.Course)
                .ThenInclude(c => c.Professor)
                .Where(e => e.StudentId == userId)
                .AsQueryable();

            Enrollments = SortOrder switch
            {
                "name_desc" => await query.OrderByDescending(e => e.Course.Title).ToListAsync(),
                "grade" => await query.OrderBy(e => e.Grade).ToListAsync(),
                "grade_desc" => await query.OrderByDescending(e => e.Grade).ToListAsync(),
                _ => await query.OrderBy(e => e.Course.Title).ToListAsync()
            };
        }
    }
}