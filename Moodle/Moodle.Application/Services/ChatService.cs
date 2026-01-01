using Moodle.Domain.Entities;
using Moodle.Domain.Repositories;

namespace Moodle.Application.Services;

public class ChatService
{
    private readonly IPrivateMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public ChatService(IPrivateMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public async Task<List<User>> GetConversationPartnersAsync(Guid userId)
    {
        return await _messageRepository.GetConversationPartnersAsync(userId);
    }

    public async Task<List<User>> GetNewChatUsersAsync(Guid currentUserId)
    {
        var allUsers = await _userRepository.GetAllExceptAsync(currentUserId);
        var partners = await _messageRepository.GetConversationPartnersAsync(currentUserId);
        var partnerIds = partners.Select(p => p.Id).ToHashSet();

        return allUsers.Where(u => !partnerIds.Contains(u.Id)).ToList();
    }

    public async Task<List<PrivateMessage>> GetConversationAsync(Guid userId, Guid otherUserId)
    {
        return await _messageRepository.GetConversationAsync(userId, otherUserId);
    }

    public async Task<(bool Success, string Message)> SendMessageAsync(
        Guid senderId,
        Guid receiverId,
        string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return (false, "Message cannot be empty");

        var message = new PrivateMessage
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        await _messageRepository.AddAsync(message);
        return (true, "Message sent");
    }
}
