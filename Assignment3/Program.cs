
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
KanbanContextFactory factory = new KanbanContextFactory();
KanbanContext context = factory.CreateDbContext(args);

var tasks1 = new Assignment3.Entities.Task{Id = 1, Title = "HewwoTwere", AssignedTo = null, Description = "uwu the owo", State = State.New, tags = new List<Tag>{}};
var tag1 = new Tag{Id = 1, Name = "HelloThere", Tasks = new List<Assignment3.Entities.Task>{}};
var user1 = new User{Id = 1, Name = "ObiWan", Email = "DankRepublic@corusant.com", Tasks = new List<Assignment3.Entities.Task>{}};
tasks1.tags.Add(tag1);
user1.Tasks.Add(tasks1);
tag1.Tasks.Add(tasks1);

context.Tags.Add(tag1);
context.Tasks.Add(tasks1);
context.Users.Add(user1);
context.Database.ExecuteSqlRaw("Select name from user where name = 'ObiWan'");

context.SaveChanges();


    

