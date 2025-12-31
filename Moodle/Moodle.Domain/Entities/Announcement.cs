namespace Moodle.Domain.Entities;

public class Announcement
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; }

    public Course Course { get; set; } = null!;
}
