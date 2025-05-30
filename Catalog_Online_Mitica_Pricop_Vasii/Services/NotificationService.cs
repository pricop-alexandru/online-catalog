using Microsoft.EntityFrameworkCore;
using Catalog_Online_Mitica_Pricop_Vasii.Data;
using Catalog_Online_Mitica_Pricop_Vasii.Models;
namespace Catalog_Online_Mitica_Pricop_Vasii.Services
{
    public class NotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task NotifyNewCourseAsync(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) return;

            var students = await _context.Enrollments
                .Where(e => e.Course.AcademicYear == DateTime.Now.Year.ToString())
                .Select(e => e.StudentId)
                .Distinct()
                .ToListAsync();

            foreach (var studentId in students)
            {
                var notification = new Notification
                {
                    UserId = studentId,
                    Message = $"You've been added to new course: {course.Title}",
                    CreatedDate = DateTime.UtcNow
                };
                _context.Notifications.Add(notification);
            }
            await _context.SaveChangesAsync();
        }

        public async Task NotifyGradeAddedAsync(int enrollmentId)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId);

            if (enrollment == null || enrollment.Student == null) return;

            var notification = new Notification
            {
                UserId = enrollment.StudentId,
                Message = $"New grade added for {enrollment.Course?.Title}: {enrollment.Grade}",
                CreatedDate = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}
