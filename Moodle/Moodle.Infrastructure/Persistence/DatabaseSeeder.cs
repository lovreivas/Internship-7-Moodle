using Microsoft.EntityFrameworkCore;
using Moodle.Domain.Entities;
using Moodle.Domain.Enums;

namespace Moodle.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(MoodleDbContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@moodle.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Role = Role.Admin,
            CreatedAt = DateTime.UtcNow
        };

        var prof1 = new User
        {
            Id = Guid.NewGuid(),
            Email = "john.professor@moodle.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("prof123"),
            Role = Role.Professor,
            CreatedAt = DateTime.UtcNow
        };

        var prof2 = new User
        {
            Id = Guid.NewGuid(),
            Email = "jane.professor@moodle.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("prof123"),
            Role = Role.Professor,
            CreatedAt = DateTime.UtcNow
        };

        var student1 = new User
        {
            Id = Guid.NewGuid(),
            Email = "alice.student@moodle.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("student123"),
            Role = Role.Student,
            CreatedAt = DateTime.UtcNow
        };

        var student2 = new User
        {
            Id = Guid.NewGuid(),
            Email = "bob.student@moodle.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("student123"),
            Role = Role.Student,
            CreatedAt = DateTime.UtcNow
        };

        var student3 = new User
        {
            Id = Guid.NewGuid(),
            Email = "charlie.student@moodle.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("student123"),
            Role = Role.Student,
            CreatedAt = DateTime.UtcNow
        };

        var student4 = new User
        {
            Id = Guid.NewGuid(),
            Email = "diana.student@moodle.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("student123"),
            Role = Role.Student,
            CreatedAt = DateTime.UtcNow
        };

        await context.Users.AddRangeAsync(admin, prof1, prof2, student1, student2, student3, student4);
        await context.SaveChangesAsync();

        var course1 = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Introduction to Programming",
            ProfessorId = prof1.Id,
            CreatedAt = DateTime.UtcNow
        };

        var course2 = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Database Systems",
            ProfessorId = prof1.Id,
            CreatedAt = DateTime.UtcNow
        };

        var course3 = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Web Development",
            ProfessorId = prof2.Id,
            CreatedAt = DateTime.UtcNow
        };

        var course4 = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Data Structures",
            ProfessorId = prof2.Id,
            CreatedAt = DateTime.UtcNow
        };

        await context.Courses.AddRangeAsync(course1, course2, course3, course4);
        await context.SaveChangesAsync();

        await context.CourseEnrollments.AddRangeAsync(
            new CourseEnrollment { Id = Guid.NewGuid(), StudentId = student1.Id, CourseId = course1.Id, EnrolledAt = DateTime.UtcNow.AddDays(-30) },
            new CourseEnrollment { Id = Guid.NewGuid(), StudentId = student1.Id, CourseId = course2.Id, EnrolledAt = DateTime.UtcNow.AddDays(-30) },
            new CourseEnrollment { Id = Guid.NewGuid(), StudentId = student1.Id, CourseId = course3.Id, EnrolledAt = DateTime.UtcNow.AddDays(-25) },
            new CourseEnrollment { Id = Guid.NewGuid(), StudentId = student2.Id, CourseId = course1.Id, EnrolledAt = DateTime.UtcNow.AddDays(-28) },
            new CourseEnrollment { Id = Guid.NewGuid(), StudentId = student2.Id, CourseId = course4.Id, EnrolledAt = DateTime.UtcNow.AddDays(-20) },
            new CourseEnrollment { Id = Guid.NewGuid(), StudentId = student3.Id, CourseId = course2.Id, EnrolledAt = DateTime.UtcNow.AddDays(-15) },
            new CourseEnrollment { Id = Guid.NewGuid(), StudentId = student3.Id, CourseId = course3.Id, EnrolledAt = DateTime.UtcNow.AddDays(-10) },
            new CourseEnrollment { Id = Guid.NewGuid(), StudentId = student4.Id, CourseId = course1.Id, EnrolledAt = DateTime.UtcNow.AddDays(-5) }
        );
        await context.SaveChangesAsync();

        await context.Announcements.AddRangeAsync(
            new Announcement { Id = Guid.NewGuid(), CourseId = course1.Id, Title = "Welcome!", Content = "Welcome to Introduction to Programming", PublishedAt = DateTime.UtcNow.AddDays(-25) },
            new Announcement { Id = Guid.NewGuid(), CourseId = course1.Id, Title = "Assignment 1", Content = "First assignment posted", PublishedAt = DateTime.UtcNow.AddDays(-20) },
            new Announcement { Id = Guid.NewGuid(), CourseId = course2.Id, Title = "SQL Workshop", Content = "Workshop next week", PublishedAt = DateTime.UtcNow.AddDays(-15) },
            new Announcement { Id = Guid.NewGuid(), CourseId = course3.Id, Title = "Project Deadline", Content = "Project due next month", PublishedAt = DateTime.UtcNow.AddDays(-10) }
        );
        await context.SaveChangesAsync();

        await context.Materials.AddRangeAsync(
            new Material { Id = Guid.NewGuid(), CourseId = course1.Id, Name = "Lecture 1 Slides", Url = "https://example.com/lecture1.pdf", UploadedAt = DateTime.UtcNow.AddDays(-25) },
            new Material { Id = Guid.NewGuid(), CourseId = course1.Id, Name = "Lab Exercise 1", Url = "https://example.com/lab1.pdf", UploadedAt = DateTime.UtcNow.AddDays(-20) },
            new Material { Id = Guid.NewGuid(), CourseId = course2.Id, Name = "SQL Tutorial", Url = "https://example.com/sql.pdf", UploadedAt = DateTime.UtcNow.AddDays(-18) },
            new Material { Id = Guid.NewGuid(), CourseId = course3.Id, Name = "HTML Guide", Url = "https://example.com/html.pdf", UploadedAt = DateTime.UtcNow.AddDays(-15) }
        );
        await context.SaveChangesAsync();

        await context.PrivateMessages.AddRangeAsync(
            new PrivateMessage { Id = Guid.NewGuid(), SenderId = student1.Id, ReceiverId = prof1.Id, Content = "Hello professor, I have a question", SentAt = DateTime.UtcNow.AddDays(-5), IsRead = true },
            new PrivateMessage { Id = Guid.NewGuid(), SenderId = prof1.Id, ReceiverId = student1.Id, Content = "Sure, how can I help?", SentAt = DateTime.UtcNow.AddDays(-5).AddHours(1), IsRead = true },
            new PrivateMessage { Id = Guid.NewGuid(), SenderId = student2.Id, ReceiverId = student3.Id, Content = "Want to study together?", SentAt = DateTime.UtcNow.AddDays(-3), IsRead = true },
            new PrivateMessage { Id = Guid.NewGuid(), SenderId = student3.Id, ReceiverId = student2.Id, Content = "Sure! Tomorrow at library?", SentAt = DateTime.UtcNow.AddDays(-3).AddHours(2), IsRead = false }
        );
        await context.SaveChangesAsync();
    }
}
