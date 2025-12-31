namespace Moodle.Domain.Entities;

public class Material
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }

    public Course Course { get; set; } = null!;
}
