namespace Catalog_Online_Mitica_Pricop_Vasii.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public ApplicationUser? Student { get; set; }
        public int? Grade { get; set; }
    }
}
