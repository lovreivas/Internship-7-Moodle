using Microsoft.EntityFrameworkCore;
using Moodle.Domain.Entities;
using Moodle.Domain.Repositories;
using Moodle.Infrastructure.Persistence;

namespace Moodle.Infrastructure.Repositories;

public class MaterialRepository : IMaterialRepository
{
    private readonly MoodleDbContext _context;

    public MaterialRepository(MoodleDbContext context)
    {
        _context = context;
    }

    public async Task<List<Material>> GetByCourseIdAsync(Guid courseId)
    {
        return await _context.Materials
            .Where(m => m.CourseId == courseId)
            .OrderByDescending(m => m.UploadedAt)
            .ToListAsync();
    }

    public async Task<Material> AddAsync(Material material)
    {
        await _context.Materials.AddAsync(material);
        await _context.SaveChangesAsync();
        return material;
    }
}
