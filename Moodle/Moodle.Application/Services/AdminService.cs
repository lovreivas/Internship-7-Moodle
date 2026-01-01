using Moodle.Domain.Entities;
using Moodle.Domain.Enums;
using Moodle.Domain.Repositories;
using System.Text.RegularExpressions;

namespace Moodle.Application.Services;

public class AdminService
{
    private readonly IUserRepository _userRepository;
    private readonly ICourseEnrollmentRepository _enrollmentRepository;
    private readonly IPrivateMessageRepository _messageRepository;

    public AdminService(
        IUserRepository userRepository,
        ICourseEnrollmentRepository enrollmentRepository,
        IPrivateMessageRepository messageRepository)
    {
        _userRepository = userRepository;
        _enrollmentRepository = enrollmentRepository;
        _messageRepository = messageRepository;
    }

    public async Task<List<User>> GetUsersByRoleAsync(Role role)
    {
        return await _userRepository.GetAllByRoleAsync(role);
    }

    public async Task<(bool Success, string Message)> DeleteUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return (false, "User not found");

        if (user.Role == Role.Admin)
            return (false, "Cannot delete admin users");

        await _messageRepository.DeleteByUserIdAsync(userId);
        await _enrollmentRepository.DeleteByUserIdAsync(userId);
        await _userRepository.DeleteAsync(user);

        return (true, "User deleted successfully");
    }

    public async Task<(bool Success, string Message)> UpdateUserEmailAsync(Guid userId, string newEmail)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return (false, "User not found");

        if (!ValidateEmail(newEmail))
            return (false, "Invalid email format");

        if (user.Email != newEmail && await _userRepository.EmailExistsAsync(newEmail))
            return (false, "Email already exists");

        user.Email = newEmail;
        await _userRepository.UpdateAsync(user);

        return (true, "Email updated successfully");
    }

    public async Task<(bool Success, string Message)> ChangeUserRoleAsync(Guid userId, Role newRole)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return (false, "User not found");

        if (user.Role == Role.Admin)
            return (false, "Cannot change admin role");

        if (newRole == Role.Admin)
            return (false, "Cannot promote to admin");

        user.Role = newRole;
        await _userRepository.UpdateAsync(user);

        return (true, $"User role changed to {newRole}");
    }

    private bool ValidateEmail(string email)
    {
        var pattern = @"^[^@\s]{1,}@[^@\s]{2,}\.[^@\s]{3,}$";
        return Regex.IsMatch(email, pattern);
    }
}
