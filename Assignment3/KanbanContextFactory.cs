using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Assignment3.Entities;

namespace Assignment3;

public class KanbanContextFactory : IDesignTimeDbContextFactory<KanbanContext>
{
    public KanbanContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().Build();
        //var connectionString = "Server=localhost,41953;User Id=postgres;Password=eikb0009;Database=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
        //optionsBuilder.UseNpgsql(connectionString);

        return new KanbanContext(optionsBuilder.Options);
    }
}