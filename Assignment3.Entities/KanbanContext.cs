namespace Assignment3.Entities;

public partial class KanbanContext : DbContext
    {
        public KanbanContext(DbContextOptions<KanbanContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users => Set<User>();
        public virtual DbSet<Tag> Tags => Set<Tag>();

        public virtual DbSet<Task> Tasks => Set<Task>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>().Property(t => t.State).HasConversion(s => s.ToString(), s => (State)Enum.Parse(typeof(State), s));
        }
    }