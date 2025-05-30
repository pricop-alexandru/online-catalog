using Catalog_Online_Mitica_Pricop_Vasii.Data;
using Catalog_Online_Mitica_Pricop_Vasii.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Catalog_Online_Mitica_Pricop_Vasii.Pages.Courses
{
    public class SearchModel : PageModel
    {  
        private readonly AppDbContext _context;

        public SearchModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Course> Courses { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string SearchType { get; set; } = "name"; // "name" or "professor"

        public async Task OnGetAsync()
        {
            var query = _context.Courses
                .Include(c => c.Professor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                if (SearchType == "professor")
                {
                    Courses = await query
                        .Where(c => c.Professor.FullName.Contains(SearchTerm))
                        .ToListAsync();
                }
                else
                {
                    Courses = await query
                        .Where(c => c.Title.Contains(SearchTerm))
                        .ToListAsync();
                }
            }
            else
            {
                Courses = await query.ToListAsync();
            }
        }
    }
}