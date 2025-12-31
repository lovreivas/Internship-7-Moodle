using Moodle.Domain.Entities;

namespace Moodle.Domain.Repositories;

public interface IPrivateMessageRepository
{
    Task<List<PrivateMessage>> GetConversationAsync(Guid userId, Guid otherUserId);
    Task<List<User>> GetConversationPartnersAsync(Guid userId);
    Task<PrivateMessage> AddAsync(PrivateMessage message);
    Task DeleteByUserIdAsync(Guid userId);
}