using Moodle.Domain.Entities;
using Moodle.Domain.Enums;

namespace Moodle.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetAllByRoleAsync(Role role);
    Task<List<User>> GetAllExceptAsync(Guid userId);
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<bool> EmailExistsAsync(string email);
}
