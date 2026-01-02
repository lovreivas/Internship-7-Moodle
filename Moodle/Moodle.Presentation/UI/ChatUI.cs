using Moodle.Application.Services;
using Moodle.Domain.Entities;

namespace Moodle.Presentation.UI;

public class ChatUI
{
    private readonly ChatService _chatService;

    public ChatUI(ChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task ShowChatMenuAsync(User user)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine("║         PRIVATE CHAT             ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("1. New Message");
            Console.WriteLine("2. My Conversations");
            Console.WriteLine("3. Back");
            Console.WriteLine();
            Console.Write("Choose option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await NewMessageAsync(user);
                    break;
                case "2":
                    await MyConversationsAsync(user);
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

    private async Task NewMessageAsync(User user)
    {
        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║         NEW MESSAGE              ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine();

        var users = await _chatService.GetNewChatUsersAsync(user.Id);

        if (users.Count == 0)
        {
            Console.WriteLine("No new users available to chat with.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < users.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {users[i].Email} ({users[i].Role})");
        }

        Console.WriteLine();
        Console.Write("Choose user (0 to cancel): ");
        var choice = Console.ReadLine();

        if (!int.TryParse(choice, out var option) || option == 0)
            return;

        if (option >= 1 && option <= users.Count)
        {
            await OpenChatAsync(user, users[option - 1]);
        }
        else
        {
            Console.WriteLine("\nInvalid selection!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private async Task MyConversationsAsync(User user)
    {
        Console.Clear();
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║      MY CONVERSATIONS            ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.WriteLine();

        var partners = await _chatService.GetConversationPartnersAsync(user.Id);

        if (partners.Count == 0)
        {
            Console.WriteLine("You have no conversations yet.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < partners.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {partners[i].Email} ({partners[i].Role})");
        }

        Console.WriteLine();
        Console.Write("Choose conversation (0 to cancel): ");
        var choice = Console.ReadLine();

        if (!int.TryParse(choice, out var option) || option == 0)
            return;

        if (option >= 1 && option <= partners.Count)
        {
            await OpenChatAsync(user, partners[option - 1]);
        }
        else
        {
            Console.WriteLine("\nInvalid selection!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private async Task OpenChatAsync(User currentUser, User otherUser)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════╗");
            Console.WriteLine($"║  Chat with: {otherUser.Email.PadRight(18)} ║");
            Console.WriteLine("╚═══════════════════════════════════╝");
            Console.WriteLine();

            var messages = await _chatService.GetConversationAsync(currentUser.Id, otherUser.Id);

            if (messages.Count == 0)
            {
                Console.WriteLine("No messages yet. Start the conversation!");
            }
            else
            {
                foreach (var message in messages)
                {
                    var senderName = message.SenderId == currentUser.Id ? "You" : message.Sender.Email;
                    Console.WriteLine($"[{message.SentAt:HH:mm}] {senderName}:");
                    Console.WriteLine($"  {message.Content}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("─────────────────────────────────────");
            Console.WriteLine("Type your message (or /exit to return):");
            Console.Write("> ");

            var input = Console.ReadLine();

            if (input == "/exit")
                return;

            if (!string.IsNullOrWhiteSpace(input))
            {
                var result = await _chatService.SendMessageAsync(currentUser.Id, otherUser.Id, input);
                if (!result.Success)
                {
                    Console.WriteLine($"\nError: {result.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}
