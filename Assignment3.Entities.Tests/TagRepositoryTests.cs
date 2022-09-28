namespace Assignment3.Entities.Tests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Assignment3.Core;

using Assignment3;

public class TagRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly TagRepository _repository;
    public TagRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        var tasks1 = new Assignment3.Entities.Task { Id = 1, Title = "HewwoTwere", AssignedTo = null, Description = "uwu the owo", State = State.New, tags = new List<Tag> { } };
        var tag1 = new Tag { Id = 1, Name = "HelloThere", Tasks = new List<Assignment3.Entities.Task> { } };
        var user1 = new User { Id = 1, Name = "ObiWan", Email = "DankRepublic@corusant.com", Tasks = new List<Assignment3.Entities.Task> { } };
        tasks1.tags.Add(tag1);
        user1.Tasks.Add(tasks1);
        tag1.Tasks.Add(tasks1);

        context.Tags.Add(tag1);
        context.Tasks.Add(tasks1);
        context.Users.Add(user1);
        context.SaveChanges();

        _context = context;
        _repository = new TagRepository(_context);
    }

    [Fact]
    public void BuisnessRule1()
    {
        var (response, created) = _repository.Create(new TagCreateDTO("myTag"));

        response.Should().Be(Response.Created);

        created.Should().Be(2);

    }

}