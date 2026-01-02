using Microsoft.EntityFrameworkCore;
using Moodle.Domain.Entities;
using Moodle.Domain.Repositories;
using Moodle.Infrastructure.Persistence;

namespace Moodle.Infrastructure.Repositories;

public class PrivateMessageRepository : IPrivateMessageRepository
{
    private readonly MoodleDbContext _context;

    public PrivateMessageRepository(MoodleDbContext context)
    {
        _context = context;
    }

    public async Task<List<PrivateMessage>> GetConversationAsync(Guid userId, Guid otherUserId)
    {
        return await _context.PrivateMessages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => (m.SenderId == userId && m.ReceiverId == otherUserId) ||
                       (m.SenderId == otherUserId && m.ReceiverId == userId))
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<List<User>> GetConversationPartnersAsync(Guid userId)
    {
        var sentToIds = await _context.PrivateMessages
            .Where(m => m.SenderId == userId)
            .Select(m => m.ReceiverId)
            .Distinct()
            .ToListAsync();

        var receivedFromIds = await _context.PrivateMessages
            .Where(m => m.ReceiverId == userId)
            .Select(m => m.SenderId)
            .Distinct()
            .ToListAsync();

        var partnerIds = sentToIds.Union(receivedFromIds).ToList();

        var partners = await _context.Users
            .Where(u => partnerIds.Contains(u.Id))
            .ToListAsync();

        var lastMessageTimes = await _context.PrivateMessages
            .Where(m => (m.SenderId == userId && partnerIds.Contains(m.ReceiverId)) ||
                       (m.ReceiverId == userId && partnerIds.Contains(m.SenderId)))
            .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
            .Select(g => new { PartnerId = g.Key, LastTime = g.Max(m => m.SentAt) })
            .ToListAsync();

        return partners
            .OrderByDescending(p => lastMessageTimes.FirstOrDefault(t => t.PartnerId == p.Id)?.LastTime ?? DateTime.MinValue)
            .ToList();
    }

    public async Task<PrivateMessage> AddAsync(PrivateMessage message)
    {
        await _context.PrivateMessages.AddAsync(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task DeleteByUserIdAsync(Guid userId)
    {
        var messages = await _context.PrivateMessages
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .ToListAsync();

        _context.PrivateMessages.RemoveRange(messages);
        await _context.SaveChangesAsync();
    }
}
