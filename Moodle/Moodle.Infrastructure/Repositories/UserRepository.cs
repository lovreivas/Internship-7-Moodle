using Microsoft.EntityFrameworkCore;
using Moodle.Domain.Entities;
using Moodle.Domain.Enums;
using Moodle.Domain.Repositories;
using Moodle.Infrastructure.Persistence;

namespace Moodle.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MoodleDbContext _context;

    public UserRepository(MoodleDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<User>> GetAllByRoleAsync(Role role)
    {
        return await _context.Users
            .Where(u => u.Role == role)
            .OrderBy(u => u.Email)
            .ToListAsync();
    }

    public async Task<List<User>> GetAllExceptAsync(Guid userId)
    {
        return await _context.Users
            .Where(u => u.Id != userId)
            .OrderBy(u => u.Email)
            .ToListAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }
}