namespace Catalog_Online_Mitica_Pricop_Vasii.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ProfessorId { get; set; } = string.Empty;
        public ApplicationUser? Professor { get; set; }
        public string AcademicYear { get; set; } = string.Empty;
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
