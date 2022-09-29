namespace Assignment3.Entities.Tests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Assignment3.Core;

public class TaskRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly TaskRepository _repository;
    public TaskRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        var user1 = new User { Id = 1, Name = "ObiWan", Email = "DankRepublic@corusant.com", Tasks = new List<Assignment3.Entities.Task> { } };
        var tasks1 = new Assignment3.Entities.Task { Id = 1, Title = "HewwoTwere", AssignedTo = user1, Description = "uwu the owo", Created = DateTime.Now, State = State.New, tags = new List<Tag> { } , StateUpdated = DateTime.Now};
        var tasks2 = new Assignment3.Entities.Task { Id = 2, Title = "Hi", AssignedTo = user1, Description = "hello there", Created = DateTime.Now,  State = State.Active, tags = new List<Tag> { } , StateUpdated = DateTime.Now};
        var tag1 = new Tag { Id = 1, Name = "HelloThere", Tasks = new List<Assignment3.Entities.Task> { } };
        tasks1.tags.Add(tag1);
        user1.Tasks.Add(tasks1);
        tag1.Tasks.Add(tasks1);

        context.Tags.Add(tag1);
        context.Tasks.Add(tasks1);
        context.Tasks.Add(tasks2);
        context.Users.Add(user1);
        context.SaveChanges();

        _context = context;
        _repository = new TaskRepository(_context);
    }

    [Fact]
    public void delete_on_non_existing_entity_returns_NotFound()
    {
        //Arrange
        var expectedRepsonse = Response.NotFound;
        //Act
        var actualResponse = _repository.Delete(254);
        //Assert
        actualResponse.Should().Be(expectedRepsonse);
    }

    [Fact]
    public void update_on_non_existing_entity_returns_NotFound()
    {
        //Arrange
        var expectedRepsonse = Response.NotFound;
        //Act
        var actualResponse = _repository.Update(new TaskUpdateDTO(234, "Laundry", 235, "Do the laundry", new List<string>(), State.New));
        //Assert
        actualResponse.Should().Be(expectedRepsonse);
    }

    [Fact]
    public void correctly_deleting_task_returns_Deleted() 
    {
        //Arrange
        var expectedRepsonse = Response.Deleted;
        //Act
        var actualResponse = _repository.Delete(1);
        //Assert
        actualResponse.Should().Be(expectedRepsonse);
    }

    [Fact]
    public void correctly_updating_task_returns_Updated()
    {
        //Arrange
        var expectedRepsonse = Response.Updated;
        //Act
        var actualResponse = _repository.Update(new TaskUpdateDTO(1, "Laundry", 1, "Do the laundry", new List<string>(), State.New));
        //Assert
        actualResponse.Should().Be(expectedRepsonse);
    }

    [Fact]
    public void deleting_Active_task_updates_state_to_removed() 
    {
        //Arrange
        var expectedState = State.Removed;
        //Act
        _repository.Delete(2);
        //Assert
        _repository.Read(2).State.Should().Be(expectedState);
    }

    [Fact]
    public void creating_task_sets_state_to_new()
    {
        //Arrange
        var expectedState = State.New;
        //Act
        _repository.Create(new TaskCreateDTO("Laundry", 1, "do the laundry!", new List<string>()));
        //Assert
        _repository.Read(3).State.Should().Be(expectedState);
    }

    [Fact]
    public void creating_task_sets_correct_times()
    {
        //Arrange
        _repository.Create(new TaskCreateDTO("Laundry", 1, "do the laundry!", new List<string>()));
        var actualTime1 = _repository.Read(2).Created;
        var actualTime2 = _repository.Read(2).StateUpdated;
        var expectedTime = DateTime.Now;

        //Assert
        actualTime1.Should().BeCloseTo(expectedTime, precision: TimeSpan.FromSeconds(5));
        actualTime2.Should().BeCloseTo(expectedTime, precision: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void updating_tags_in_task()
    {
        //Arrange
        var expectedTags = new List<string>(){"HelloThere"};
        //Act
        _repository.Update(new TaskUpdateDTO(1, "Laundry", 1, "Do the laundry", new List<string>(){"HelloThere"}, State.Resolved));
        //Assert
        _repository.Read(1).Tags.Should().BeEquivalentTo(expectedTags);
    }

    [Fact]
    public void updating_state_of_task_updates_time()
    {
        //Arrange
        _repository.Update(new TaskUpdateDTO(1, "Laundry", 1, "Do the laundry", new List<string>(){"HelloThere"}, State.Resolved));
        DateTime expectedTime = DateTime.Now;
        //Act
        DateTime actualTime = _repository.Read(1).StateUpdated;
        //Assert
        actualTime.Should().BeCloseTo(expectedTime, precision: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void assigning_non_existing_user_returns_bad_request() 
    {
        //Arrange
        var expectedRepsonse = Response.BadRequest;
        //Act
        var actualResponse = _repository.Update(new TaskUpdateDTO(1, "Laundry", 2, "Do the laundry", new List<string>(){"HelloThere"}, State.Resolved));
        //Assert
        actualResponse.Should().Be(expectedRepsonse);
    }

    [Fact]
    public void read_returns_null_when_searching_for_nonexisting_task()
    {
        //Act
        TaskDetailsDTO Actual = _repository.Read(25);
        //Assert
        Actual.Should().BeNull();
    }

    [Fact]
    public void read_all_returns_correct_elements() 
    {
        //Arrange
        var expectedLength = 2;
        //Act
        var actualList = _repository.ReadAll();
        //Assert
        actualList.Count().Should().Be(expectedLength);
    }

    [Fact]
    public void read_all_removed_returns_correct_output()
    {
        //Arrange
        var expectedLength = 1;
        //Act
        _repository.Delete(2);
        var actualList = _repository.ReadAllRemoved();
        //Assert
        actualList.Count().Should().Be(expectedLength);
    }
}
