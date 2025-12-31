using Moodle.Domain.Entities;

namespace Moodle.Domain.Repositories;

public interface IAnnouncementRepository
{
    Task<List<Announcement>> GetByCourseIdAsync(Guid courseId);
    Task<Announcement> AddAsync(Announcement announcement);
}