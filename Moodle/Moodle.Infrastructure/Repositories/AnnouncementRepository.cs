using Microsoft.EntityFrameworkCore;
using Moodle.Domain.Entities;
using Moodle.Domain.Repositories;
using Moodle.Infrastructure.Persistence;

namespace Moodle.Infrastructure.Repositories;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly MoodleDbContext _context;

    public AnnouncementRepository(MoodleDbContext context)
    {
        _context = context;
    }

    public async Task<List<Announcement>> GetByCourseIdAsync(Guid courseId)
    {
        return await _context.Announcements
            .Where(a => a.CourseId == courseId)
            .OrderByDescending(a => a.PublishedAt)
            .ToListAsync();
    }

    public async Task<Announcement> AddAsync(Announcement announcement)
    {
        await _context.Announcements.AddAsync(announcement);
        await _context.SaveChangesAsync();
        return announcement;
    }
}
