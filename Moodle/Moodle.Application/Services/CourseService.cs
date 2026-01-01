using Moodle.Domain.Entities;
using Moodle.Domain.Enums;
using Moodle.Domain.Repositories;

namespace Moodle.Application.Services;

public class CourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICourseEnrollmentRepository _enrollmentRepository;
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IUserRepository _userRepository;

    public CourseService(
        ICourseRepository courseRepository,
        ICourseEnrollmentRepository enrollmentRepository,
        IAnnouncementRepository announcementRepository,
        IMaterialRepository materialRepository,
        IUserRepository userRepository)
    {
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _announcementRepository = announcementRepository;
        _materialRepository = materialRepository;
        _userRepository = userRepository;
    }

    public async Task<List<Course>> GetStudentCoursesAsync(Guid studentId)
    {
        return await _courseRepository.GetByStudentIdAsync(studentId);
    }

    public async Task<List<Course>> GetProfessorCoursesAsync(Guid professorId)
    {
        return await _courseRepository.GetByProfessorIdAsync(professorId);
    }

    public async Task<Course?> GetCourseByIdAsync(Guid courseId)
    {
        return await _courseRepository.GetByIdAsync(courseId);
    }

    public async Task<List<User>> GetCourseStudentsAsync(Guid courseId)
    {
        return await _courseRepository.GetEnrolledStudentsAsync(courseId);
    }

    public async Task<List<Announcement>> GetCourseAnnouncementsAsync(Guid courseId)
    {
        return await _announcementRepository.GetByCourseIdAsync(courseId);
    }

    public async Task<List<Material>> GetCourseMaterialsAsync(Guid courseId)
    {
        return await _materialRepository.GetByCourseIdAsync(courseId);
    }

    public async Task<List<User>> GetAllStudentsAsync()
    {
        return await _userRepository.GetAllByRoleAsync(Role.Student);
    }

    public async Task<(bool Success, string Message)> EnrollStudentAsync(Guid studentId, Guid courseId)
    {
        if (await _enrollmentRepository.IsEnrolledAsync(studentId, courseId))
            return (false, "Student is already enrolled in this course");

        var enrollment = new CourseEnrollment
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            CourseId = courseId,
            EnrolledAt = DateTime.UtcNow
        };

        await _enrollmentRepository.AddAsync(enrollment);
        return (true, "Student enrolled successfully");
    }

    public async Task<(bool Success, string Message)> PublishAnnouncementAsync(
        Guid courseId,
        string title,
        string content)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            return (false, "Title and content are required");

        var announcement = new Announcement
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Title = title,
            Content = content,
            PublishedAt = DateTime.UtcNow
        };

        await _announcementRepository.AddAsync(announcement);
        return (true, "Announcement published successfully");
    }

    public async Task<(bool Success, string Message)> AddMaterialAsync(
        Guid courseId,
        string name,
        string url)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url))
            return (false, "Name and URL are required");

        if (!IsValidUrl(url))
            return (false, "Invalid URL format");

        var material = new Material
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Name = name,
            Url = url,
            UploadedAt = DateTime.UtcNow
        };

        await _materialRepository.AddAsync(material);
        return (true, "Material added successfully");
    }

    private bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var result) &&
               (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}
