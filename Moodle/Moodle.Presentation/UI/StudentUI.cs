using Moodle.Application.Services;
using Moodle.Domain.Entities;

namespace Moodle.Presentation.UI;

public class StudentUI
{
    private readonly CourseService _courseService;

    public StudentUI(CourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task ShowMyCoursesAsync(User student)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine("║          MY COURSES              ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.WriteLine();

            var courses = await _courseService.GetStudentCoursesAsync(student.Id);

            if (courses.Count == 0)
            {
                Console.WriteLine("You are not enrolled in any courses.");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < courses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {courses[i].Name} (Prof: {courses[i].Professor.Email})");
            }

            Console.WriteLine($"\n{courses.Count + 1}. Back");
            Console.WriteLine();
            Console.Write("Choose course: ");

            var choice = Console.ReadLine();

            if (!int.TryParse(choice, out var option))
            {
                Console.WriteLine("\nInvalid input!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                continue;
            }

            if (option == courses.Count + 1)
                return;

            if (option >= 1 && option <= courses.Count)
            {
                await ShowCourseDetailsAsync(courses[option - 1]);
            }
            else
            {
                Console.WriteLine("\nInvalid option!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private async Task ShowCourseDetailsAsync(Course course)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine($"║  {course.Name.PadRight(30)}  ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("1. View Announcements");
            Console.WriteLine("2. View Materials");
            Console.WriteLine("3. Back");
            Console.WriteLine();
            Console.Write("Choose option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ShowAnnouncementsAsync(course.Id, course.Professor.Email);
                    break;
                case "2":
                    await ShowMaterialsAsync(course.Id);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("\nInvalid option!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task ShowAnnouncementsAsync(Guid courseId, string professorEmail)
    {
        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║        ANNOUNCEMENTS             ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine();

        var announcements = await _courseService.GetCourseAnnouncementsAsync(courseId);

        if (announcements.Count == 0)
        {
            Console.WriteLine("No announcements yet.");
        }
        else
        {
            foreach (var announcement in announcements)
            {
                Console.WriteLine($"[{announcement.PublishedAt:dd/MM/yyyy HH:mm}] by {professorEmail}");
                Console.WriteLine($"Title: {announcement.Title}");
                Console.WriteLine($"Content: {announcement.Content}");
                Console.WriteLine("─────────────────────────────────────");
            }
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private async Task ShowMaterialsAsync(Guid courseId)
    {
        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║          MATERIALS               ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine();

        var materials = await _courseService.GetCourseMaterialsAsync(courseId);

        if (materials.Count == 0)
        {
            Console.WriteLine("No materials yet.");
        }
        else
        {
            foreach (var material in materials)
            {
                Console.WriteLine($"[{material.UploadedAt:dd/MM/yyyy HH:mm}]");
                Console.WriteLine($"Name: {material.Name}");
                Console.WriteLine($"URL: {material.Url}");
                Console.WriteLine("─────────────────────────────────────");
            }
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }
}