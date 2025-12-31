using Moodle.Domain.Entities;

namespace Moodle.Domain.Repositories;

public interface ICourseEnrollmentRepository
{
    Task<bool> IsEnrolledAsync(Guid studentId, Guid courseId);
    Task<CourseEnrollment> AddAsync(CourseEnrollment enrollment);
    Task DeleteByUserIdAsync(Guid userId);
}
