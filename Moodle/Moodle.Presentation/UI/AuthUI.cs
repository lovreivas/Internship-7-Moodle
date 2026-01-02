using Moodle.Application.Services;

namespace Moodle.Presentation.UI;

public class AuthUI
{
    private readonly AuthService _authService;
    private readonly MainMenuUI _mainMenuUI;

    public AuthUI(AuthService authService, MainMenuUI mainMenuUI)
    {
        _authService = authService;
        _mainMenuUI = mainMenuUI;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine("║        MOODLE SYSTEM             ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
            Console.WriteLine();
            Console.Write("Choose option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await HandleLoginAsync();
                    break;
                case "2":
                    await HandleRegisterAsync();
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

    private async Task HandleLoginAsync()
    {
        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║            LOGIN                 ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine();

        Console.Write("Email: ");
        var email = Console.ReadLine() ?? "";

        Console.Write("Password: ");
        var password = ReadPassword();

        var user = await _authService.LoginAsync(email, password);

        if (user != null)
        {
            Console.WriteLine("\nLogin successful!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            await _mainMenuUI.ShowAsync(user);
        }
        else
        {
            Console.WriteLine("\nInvalid email or password!");
            Console.WriteLine("\nWaiting 30 seconds before retry...");
            await Task.Delay(30000);
        }
    }

    private async Task HandleRegisterAsync()
    {
        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║         REGISTRATION             ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine();

        Console.Write("Email: ");
        var email = Console.ReadLine() ?? "";

        Console.Write("Password: ");
        var password = ReadPassword();

        Console.Write("\nConfirm Password: ");
        var confirmPassword = ReadPassword();

        var captcha = _authService.GenerateCaptcha();
        Console.WriteLine($"\n\nCaptcha: {captcha}");
        Console.Write("Enter captcha: ");
        var captchaInput = Console.ReadLine() ?? "";

        var result = await _authService.RegisterAsync(email, password, confirmPassword, captchaInput, captcha);

        Console.WriteLine($"\n{result.Message}");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private string ReadPassword()
    {
        var password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[0..^1];
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        return password;
    }
}
