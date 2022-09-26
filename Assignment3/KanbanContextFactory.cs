using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Assignment3.Entities;
using Npgsql;

namespace Assignment3;

internal class KanbanContextFactory : IDesignTimeDbContextFactory<KanbanContext>
{
    public KanbanContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().Build();
        var connectionString = "Server=172.17.0.1;Database=49153;User Id=postgres;Password=postgrespw;";
        

        var connString = "Host=49153;Username=postgres;Password=postgrespw;Database=postgres";
        using var conn = new NpgsqlConnection(connString);
        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
        optionsBuilder.UseSqlServer(connectionString);
        //optionsBuilder.UseSqlServer(conn);
        
        return new KanbanContext(optionsBuilder.Options);
    }
}