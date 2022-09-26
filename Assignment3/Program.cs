// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
KanbanContextFactory factory = new KanbanContextFactory();

KanbanContext context = factory.CreateDbContext(args);
var facade = context.Database;
Console.WriteLine(facade.CanConnect());
