using Microsoft.EntityFrameworkCore;
using Moodle.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Moodle.Infrastructure.Persistence;

public class MoodleDbContext : DbContext
{
    public MoodleDbContext(DbContextOptions<MoodleDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseEnrollment> CourseEnrollments => Set<CourseEnrollment>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<PrivateMessage> PrivateMessages => Set<PrivateMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasMany(e => e.CoursesAsProfessor)
                .WithOne(e => e.Professor)
                .HasForeignKey(e => e.ProfessorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Enrollments)
                .WithOne(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.SentMessages)
                .WithOne(e => e.Sender)
                .HasForeignKey(e => e.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.ReceivedMessages)
                .WithOne(e => e.Receiver)
                .HasForeignKey(e => e.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("courses");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ProfessorId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasMany(e => e.Enrollments)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Announcements)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Materials)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CourseEnrollment>(entity =>
        {
            entity.ToTable("course_enrollments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StudentId).IsRequired();
            entity.Property(e => e.CourseId).IsRequired();
            entity.Property(e => e.EnrolledAt).IsRequired();
            entity.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();
        });

        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.ToTable("announcements");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CourseId).IsRequired();
            entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.PublishedAt).IsRequired();
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.ToTable("materials");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CourseId).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Url).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.UploadedAt).IsRequired();
        });

        modelBuilder.Entity<PrivateMessage>(entity =>
        {
            entity.ToTable("private_messages");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SenderId).IsRequired();
            entity.Property(e => e.ReceiverId).IsRequired();
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.SentAt).IsRequired();
            entity.Property(e => e.IsRead).IsRequired();
            entity.HasIndex(e => new { e.SenderId, e.ReceiverId });
        });
    }
}
