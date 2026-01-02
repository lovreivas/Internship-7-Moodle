using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moodle.Application.Services;
using Moodle.Domain.Repositories;
using Moodle.Infrastructure.Persistence;
using Moodle.Infrastructure.Repositories;
using Moodle.Presentation.UI;

var services = new ServiceCollection();

services.AddDbContext<MoodleDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=moodle;Username=postgres;Password=postgres"));

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<ICourseRepository, CourseRepository>();
services.AddScoped<ICourseEnrollmentRepository, CourseEnrollmentRepository>();
services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
services.AddScoped<IMaterialRepository, MaterialRepository>();
services.AddScoped<IPrivateMessageRepository, PrivateMessageRepository>();

services.AddScoped<AuthService>();
services.AddScoped<CourseService>();
services.AddScoped<ChatService>();
services.AddScoped<AdminService>();

services.AddScoped<AuthUI>();
services.AddScoped<MainMenuUI>();
services.AddScoped<StudentUI>();
services.AddScoped<ProfessorUI>();
services.AddScoped<AdminUI>();
//services.AddScoped<ChatUI>();

var serviceProvider = services.BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MoodleDbContext>();
    await context.Database.MigrateAsync();
    await DatabaseSeeder.SeedAsync(context);
}

var authUI = serviceProvider.GetRequiredService<AuthUI>();
await authUI.StartAsync();

Console.WriteLine("\nApplication closed. Press any key to exit...");
Console.ReadKey();
