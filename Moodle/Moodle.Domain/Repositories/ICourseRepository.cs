using Moodle.Domain.Entities;

namespace Moodle.Domain.Repositories;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id);
    Task<Course?> GetByIdWithDetailsAsync(Guid id);
    Task<List<Course>> GetByProfessorIdAsync(Guid professorId);
    Task<List<Course>> GetByStudentIdAsync(Guid studentId);
    Task<List<User>> GetEnrolledStudentsAsync(Guid courseId);
    Task<Course> AddAsync(Course course);
    Task DeleteAsync(Course course);
}
