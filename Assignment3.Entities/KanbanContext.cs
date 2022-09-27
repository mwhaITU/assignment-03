namespace Assignment3.Entities;
using Assignment3.Core;
public partial class KanbanContext : DbContext
{
    public KanbanContext(DbContextOptions<KanbanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<Tag> Tags => Set<Tag>();

    public virtual DbSet<Task> Tasks => Set<Task>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Server=localhost,41953;User Id=postgres;Password=ebbp0009;Database=postgres", 
        options => options.UseAdminDatabase("helloThere"));
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>().Property(t => t.State).HasConversion(s => s.ToString(), s => (State)Enum.Parse(typeof(State), s));
    }
}