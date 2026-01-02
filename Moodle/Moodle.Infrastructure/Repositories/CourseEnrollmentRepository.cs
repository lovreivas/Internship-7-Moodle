using Microsoft.EntityFrameworkCore;
using Moodle.Domain.Entities;
using Moodle.Domain.Repositories;
using Moodle.Infrastructure.Persistence;

namespace Moodle.Infrastructure.Repositories;

public class CourseEnrollmentRepository : ICourseEnrollmentRepository
{
    private readonly MoodleDbContext _context;

    public CourseEnrollmentRepository(MoodleDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsEnrolledAsync(Guid studentId, Guid courseId)
    {
        return await _context.CourseEnrollments
            .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
    }

    public async Task<CourseEnrollment> AddAsync(CourseEnrollment enrollment)
    {
        await _context.CourseEnrollments.AddAsync(enrollment);
        await _context.SaveChangesAsync();
        return enrollment;
    }

    public async Task DeleteByUserIdAsync(Guid userId)
    {
        var enrollments = await _context.CourseEnrollments
            .Where(e => e.StudentId == userId)
            .ToListAsync();

        _context.CourseEnrollments.RemoveRange(enrollments);
        await _context.SaveChangesAsync();
    }
}
