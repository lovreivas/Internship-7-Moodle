using Moodle.Application.Services;
using Moodle.Domain.Entities;

namespace Moodle.Presentation.UI;

public class ProfessorUI
{
    private readonly CourseService _courseService;

    public ProfessorUI(CourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task ShowMyCoursesAsync(User professor)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine("║          MY COURSES              ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.WriteLine();

            var courses = await _courseService.GetProfessorCoursesAsync(professor.Id);

            if (courses.Count == 0)
            {
                Console.WriteLine("You are not teaching any courses.");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < courses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {courses[i].Name}");
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
                await ShowCourseViewAsync(courses[option - 1]);
            }
            else
            {
                Console.WriteLine("\nInvalid option!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private async Task ShowCourseViewAsync(Course course)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine($"║  {course.Name.PadRight(30)}  ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("1. View Students");
            Console.WriteLine("2. View Announcements");
            Console.WriteLine("3. View Materials");
            Console.WriteLine("4. Back");
            Console.WriteLine();
            Console.Write("Choose option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ShowStudentsAsync(course.Id);
                    break;
                case "2":
                    await ShowAnnouncementsAsync(course.Id);
                    break;
                case "3":
                    await ShowMaterialsAsync(course.Id);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("\nInvalid option!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    public async Task ShowManageCoursesAsync(User professor)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine("║       MANAGE COURSES             ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.WriteLine();

            var courses = await _courseService.GetProfessorCoursesAsync(professor.Id);

            if (courses.Count == 0)
            {
                Console.WriteLine("You are not teaching any courses.");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < courses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {courses[i].Name}");
            }

            Console.WriteLine($"\n{courses.Count + 1}. Back");
            Console.WriteLine();
            Console.Write("Choose course to manage: ");

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
                await ShowCourseManagementAsync(courses[option - 1]);
            }
            else
            {
                Console.WriteLine("\nInvalid option!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private async Task ShowCourseManagementAsync(Course course)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine("║     COURSE MANAGEMENT            ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine($"Course: {course.Name}");
            Console.WriteLine();
            Console.WriteLine("1. Add Student to Course");
            Console.WriteLine("2. Publish Announcement");
            Console.WriteLine("3. Add Material");
            Console.WriteLine("4. Back");
            Console.WriteLine();
            Console.Write("Choose option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await AddStudentToCourseAsync(course.Id);
                    break;
                case "2":
                    await PublishAnnouncementAsync(course.Id);
                    break;
                case "3":
                    await AddMaterialAsync(course.Id);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("\nInvalid option!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task AddStudentToCourseAsync(Guid courseId)
    {
        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║      ADD STUDENT TO COURSE       ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine();

        var students = await _courseService.GetAllStudentsAsync();

        if (students.Count == 0)
        {
            Console.WriteLine("No students available.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < students.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {students[i].Email}");
        }

        Console.WriteLine();
        Console.Write("Choose student (0 to cancel): ");
        var choice = Console.ReadLine();

        if (!int.TryParse(choice, out var option) || option == 0)
            return;

        if (option >= 1 && option <= students.Count)
        {
            var result = await _courseService.EnrollStudentAsync(students[option - 1].Id, courseId);
            Console.WriteLine($"\n{result.Message}");
        }
        else
        {
            Console.WriteLine("\nInvalid selection!");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task PublishAnnouncementAsync(Guid courseId)
    {
        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║     PUBLISH ANNOUNCEMENT         ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine();

        Console.Write("Title: ");
        var title = Console.ReadLine() ?? "";

        Console.Write("Content: ");
        var content = Console.ReadLine() ?? "";

        var result = await _courseService.PublishAnnouncementAsync(courseId, title, content);
        Console.WriteLine($"\n{result.Message}");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task AddMaterialAsync(Guid courseId)
    {
        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║         ADD MATERIAL             ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine();

        Console.Write("Material Name: ");
        var name = Console.ReadLine() ?? "";

        Console.Write("URL: ");
        var url = Console.ReadLine() ?? "";

        var result = await _courseService.AddMaterialAsync(courseId, name, url);
        Console.WriteLine($"\n{result.Message}");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task ShowStudentsAsync(Guid courseId)
    {
        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║      ENROLLED STUDENTS           ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine();

        var students = await _courseService.GetCourseStudentsAsync(courseId);

        if (students.Count == 0)
        {
            Console.WriteLine("No students enrolled.");
        }
        else
        {
            foreach (var student in students)
            {
                Console.WriteLine($"- {student.Email}");
            }
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private async Task ShowAnnouncementsAsync(Guid courseId)
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
                Console.WriteLine($"[{announcement.PublishedAt:dd/MM/yyyy HH:mm}]");
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
