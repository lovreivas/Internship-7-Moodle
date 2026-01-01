using Microsoft.EntityFrameworkCore;
using Moodle.Domain.Entities;
using Moodle.Domain.Repositories;
using Moodle.Infrastructure.Persistence;

namespace Moodle.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly MoodleDbContext _context;

    public CourseRepository(MoodleDbContext context)
    {
        _context = context;
    }

    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _context.Courses
            .Include(c => c.Professor)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Course?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _context.Courses
            .Include(c => c.Professor)
            .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
            .Include(c => c.Announcements)
            .Include(c => c.Materials)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Course>> GetByProfessorIdAsync(Guid professorId)
    {
        return await _context.Courses
            .Where(c => c.ProfessorId == professorId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<List<Course>> GetByStudentIdAsync(Guid studentId)
    {
        return await _context.CourseEnrollments
            .Where(e => e.StudentId == studentId)
            .Include(e => e.Course)
                .ThenInclude(c => c.Professor)
            .Select(e => e.Course)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<List<User>> GetEnrolledStudentsAsync(Guid courseId)
    {
        return await _context.CourseEnrollments
            .Where(e => e.CourseId == courseId)
            .Include(e => e.Student)
            .Select(e => e.Student)
            .OrderBy(s => s.Email)
            .ToListAsync();
    }

    public async Task<Course> AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task DeleteAsync(Course course)
    {
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
    }
}