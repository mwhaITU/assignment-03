// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
KanbanContextFactory factory = new KanbanContextFactory();

KanbanContext context = factory.CreateDbContext(args);
var facade = context.Database;
//facade.OpenConnection();

var connString = "Host=localhost,41953;Username=postgres;Password=eikb0009;Database=homeworkOne";


await using var conn = new NpgsqlConnection(connString);
await conn.OpenAsync();
await using var cmd = new NpgsqlCommand("Select id from person", conn);
await using var reader = await cmd.ExecuteReaderAsync();

while (await reader.ReadAsync())
{
    Console.WriteLine(reader.GetInt64(0));
}


