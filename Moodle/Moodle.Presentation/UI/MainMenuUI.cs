using Moodle.Domain.Entities;
using Moodle.Domain.Enums;

namespace Moodle.Presentation.UI;

public class MainMenuUI
{
    private readonly StudentUI _studentUI;
    private readonly ProfessorUI _professorUI;
    private readonly AdminUI _adminUI;
    private readonly ChatUI _chatUI;

    public MainMenuUI(
        StudentUI studentUI,
        ProfessorUI professorUI,
        AdminUI adminUI,
        ChatUI chatUI)
    {
        _studentUI = studentUI;
        _professorUI = professorUI;
        _adminUI = adminUI;
        _chatUI = chatUI;
    }

    public async Task ShowAsync(User user)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine("║          MAIN MENU               ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine($"Logged in as: {user.Email}");
            Console.WriteLine($"Role: {user.Role}");
            Console.WriteLine();

            var options = new List<string>();

            if (user.Role == Role.Student)
            {
                options.Add("My Courses");
            }
            else if (user.Role == Role.Professor)
            {
                options.Add("My Courses");
                options.Add("Manage Courses");
            }
            else if (user.Role == Role.Admin)
            {
                options.Add("Manage Users");
            }

            options.Add("Private Chat");
            options.Add("Logout");

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            Console.WriteLine();
            Console.Write("Choose option: ");
            var choice = Console.ReadLine();

            if (!int.TryParse(choice, out var option) || option < 1 || option > options.Count)
            {
                Console.WriteLine("\nInvalid option!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                continue;
            }

            var selected = options[option - 1];

            switch (selected)
            {
                case "My Courses":
                    if (user.Role == Role.Student)
                        await _studentUI.ShowMyCoursesAsync(user);
                    else if (user.Role == Role.Professor)
                        await _professorUI.ShowMyCoursesAsync(user);
                    break;
                case "Manage Courses":
                    await _professorUI.ShowManageCoursesAsync(user);
                    break;
                case "Manage Users":
                    await _adminUI.ShowManageUsersAsync(user);
                    break;
                case "Private Chat":
                    await _chatUI.ShowChatMenuAsync(user);
                    break;
                case "Logout":
                    return;
            }
        }
    }
}
