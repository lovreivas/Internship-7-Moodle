using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Moodle.Infrastructure.Persistence;

public class MoodleDbContextFactory : IDesignTimeDbContextFactory<MoodleDbContext>
{
    public MoodleDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MoodleDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=moodle;Username=postgres;Password=postgres");

        return new MoodleDbContext(optionsBuilder.Options);
    }
}