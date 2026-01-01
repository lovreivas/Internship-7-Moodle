using Moodle.Domain.Entities;
using Moodle.Domain.Enums;
using Moodle.Domain.Repositories;
using System.Text.RegularExpressions;

namespace Moodle.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return user;
    }

    public async Task<(bool Success, string Message, User? User)> RegisterAsync(
        string email,
        string password,
        string confirmPassword,
        string captchaInput,
        string expectedCaptcha)
    {
        if (!ValidateEmail(email))
            return (false, "Invalid email format. Required: [text]@[text].[text]", null);

        if (await _userRepository.EmailExistsAsync(email))
            return (false, "Email already exists", null);

        if (password != confirmPassword)
            return (false, "Passwords do not match", null);

        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            return (false, "Password must be at least 6 characters", null);

        if (captchaInput != expectedCaptcha)
            return (false, "Incorrect captcha", null);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = Role.Student,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        return (true, "Registration successful", user);
    }

    public string GenerateCaptcha()
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        string captcha;
        do
        {
            captcha = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        } while (!captcha.Any(char.IsLetter) || !captcha.Any(char.IsDigit));

        return captcha;
    }

    private bool ValidateEmail(string email)
    {
        var pattern = @"^[^@\s]{1,}@[^@\s]{2,}\.[^@\s]{3,}$";
        return Regex.IsMatch(email, pattern);
    }
}