using Moodle.Application.Services;
using Moodle.Domain.Entities;
using Moodle.Domain.Enums;

namespace Moodle.Presentation.UI;

public class AdminUI
{
    private readonly AdminService _adminService;

    public AdminUI(AdminService adminService)
    {
        _adminService = adminService;
    }

    public async Task ShowManageUsersAsync(User admin)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine("║        MANAGE USERS              ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("1. Delete User");
            Console.WriteLine("2. Update User Email");
            Console.WriteLine("3. Change User Role");
            Console.WriteLine("4. Back");
            Console.WriteLine();
            Console.Write("Choose option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await DeleteUserAsync();
                    break;
                case "2":
                    await UpdateUserEmailAsync();
                    break;
                case "3":
                    await ChangeUserRoleAsync();
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

    private async Task DeleteUserAsync()
    {
        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║         DELETE USER              ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("Select user type:");
        Console.WriteLine("1. Students");
        Console.WriteLine("2. Professors");
        Console.WriteLine("3. Cancel");
        Console.WriteLine();
        Console.Write("Choose option: ");

        var typeChoice = Console.ReadLine();
        Role? role = typeChoice switch
        {
            "1" => Role.Student,
            "2" => Role.Professor,
            _ => null
        };

        if (!role.HasValue)
            return;

        var users = await _adminService.GetUsersByRoleAsync(role.Value);

        if (users.Count == 0)
        {
            Console.WriteLine($"\nNo {role}s found.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        Console.WriteLine($"╔═══════════════════════════════════╗");
        Console.WriteLine($"║    DELETE {role.ToString().ToUpper().PadRight(20)} ║");
        Console.WriteLine($"╚═══════════════════════════════════╝");
        Console.WriteLine();

        for (int i = 0; i < users.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {users[i].Email}");
        }

        Console.WriteLine();
        Console.Write("Choose user to delete (0 to cancel): ");
        var choice = Console.ReadLine();

        if (!int.TryParse(choice, out var option) || option == 0)
            return;

        if (option >= 1 && option <= users.Count)
        {
            var result = await _adminService.DeleteUserAsync(users[option - 1].Id);
            Console.WriteLine($"\n{result.Message}");
        }
        else
        {
            Console.WriteLine("\nInvalid selection!");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task UpdateUserEmailAsync()
    {
        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║      UPDATE USER EMAIL           ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("Select user type:");
        Console.WriteLine("1. Students");
        Console.WriteLine("2. Professors");
        Console.WriteLine("3. Cancel");
        Console.WriteLine();
        Console.Write("Choose option: ");

        var typeChoice = Console.ReadLine();
        Role? role = typeChoice switch
        {
            "1" => Role.Student,
            "2" => Role.Professor,
            _ => null
        };

        if (!role.HasValue)
            return;

        var users = await _adminService.GetUsersByRoleAsync(role.Value);

        if (users.Count == 0)
        {
            Console.WriteLine($"\nNo {role}s found.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        Console.WriteLine($"╔═══════════════════════════════════╗");
        Console.WriteLine($"║   UPDATE {role.ToString().ToUpper().PadRight(20)} EMAIL ║");
        Console.WriteLine($"╚═══════════════════════════════════╝");
        Console.WriteLine();

        for (int i = 0; i < users.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {users[i].Email}");
        }

        Console.WriteLine();
        Console.Write("Choose user (0 to cancel): ");
        var choice = Console.ReadLine();

        if (!int.TryParse(choice, out var option) || option == 0)
            return;

        if (option >= 1 && option <= users.Count)
        {
            Console.Write("\nNew email: ");
            var newEmail = Console.ReadLine() ?? "";

            var result = await _adminService.UpdateUserEmailAsync(users[option - 1].Id, newEmail);
            Console.WriteLine($"\n{result.Message}");
        }
        else
        {
            Console.WriteLine("\nInvalid selection!");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task ChangeUserRoleAsync()
    {
        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║       CHANGE USER ROLE           ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("Select user type:");
        Console.WriteLine("1. Students (promote to Professor)");
        Console.WriteLine("2. Professors (demote to Student)");
        Console.WriteLine("3. Cancel");
        Console.WriteLine();
        Console.Write("Choose option: ");

        var typeChoice = Console.ReadLine();
        Role? currentRole = typeChoice switch
        {
            "1" => Role.Student,
            "2" => Role.Professor,
            _ => null
        };

        if (!currentRole.HasValue)
            return;

        var users = await _adminService.GetUsersByRoleAsync(currentRole.Value);

        if (users.Count == 0)
        {
            Console.WriteLine($"\nNo {currentRole}s found.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        Console.WriteLine($"╔═══════════════════════════════════╗");
        Console.WriteLine($"║   CHANGE {currentRole.ToString().ToUpper().PadRight(20)} ROLE ║");
        Console.WriteLine($"╚═══════════════════════════════════╝");
        Console.WriteLine();

        for (int i = 0; i < users.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {users[i].Email}");
        }

        Console.WriteLine();
        Console.Write("Choose user (0 to cancel): ");
        var choice = Console.ReadLine();

        if (!int.TryParse(choice, out var option) || option == 0)
            return;

        if (option >= 1 && option <= users.Count)
        {
            var newRole = currentRole.Value == Role.Student ? Role.Professor : Role.Student;
            Console.WriteLine($"\nChange role to {newRole}? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();

            if (confirm == "y")
            {
                var result = await _adminService.ChangeUserRoleAsync(users[option - 1].Id, newRole);
                Console.WriteLine($"\n{result.Message}");
            }
            else
            {
                Console.WriteLine("\nCancelled.");
            }
        }
        else
        {
            Console.WriteLine("\nInvalid selection!");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}
