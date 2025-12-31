using Moodle.Domain.Entities;

namespace Moodle.Domain.Repositories;

public interface IMaterialRepository
{
    Task<List<Material>> GetByCourseIdAsync(Guid courseId);
    Task<Material> AddAsync(Material material);
}
