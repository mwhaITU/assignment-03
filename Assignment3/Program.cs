// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
KanbanContextFactory factory = new KanbanContextFactory();

KanbanContext context = factory.CreateDbContext(args);
var facade = context.Database;
//facade.OpenConnection();

var connString = "Host=localhost,41953;Username=postgres;Password=eikb0009;Database=postgres";


await using var conn = new NpgsqlConnection(connString);
await conn.OpenAsync();
