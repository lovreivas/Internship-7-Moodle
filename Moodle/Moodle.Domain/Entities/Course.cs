namespace Moodle.Domain.Entities;

public class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid ProfessorId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User Professor { get; set; } = null!;
    public ICollection<CourseEnrollment> Enrollments { get; set; } = new List<CourseEnrollment>();
    public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
    public ICollection<Material> Materials { get; set; } = new List<Material>();
}