namespace Assignment3.Entities;
using System.Collections.ObjectModel;

public class TaskRepository : ITaskRepository
{
    private readonly KanbanContext _context;
    public TaskRepository(KanbanContext context)
    {
        _context = context;
    }

    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        Response response;
        int taskId;
        ICollection<Tag> tagsList = new List<Tag>();
        foreach (var s in task.Tags)
        {
            var tagsQuery = _context.Tags.Where(t => t.Name == s).Select(t => t);
            foreach (var t in tagsQuery)
            {
                tagsList.Add(t);
            }
        }
        Task entity = new Task
        {
            Title = task.Title,
            AssignedTo = _context.Users.Find(task.AssignedToId),
            Description = task.Description,
            Created = DateTime.Now,
            State = State.New,
            tags = tagsList,
            StateUpdated = DateTime.Now
        };
        if (entity.AssignedTo == null)
        {
            return (Response.BadRequest, entity.Id);
        }
        _context.Tasks.Add(entity);
        _context.SaveChanges();
        response = Response.Created;
        return (response, entity.Id);
    }
    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var taskCollection =    from t in _context.Tasks
                                select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, t.tags.Select(x => new string(x.Name)).ToArray(), t.State);
        return new ReadOnlyCollection<TaskDTO>(taskCollection.ToList());
    }
    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        var taskCollection =    from t in _context.Tasks
                                where t.State == State.Removed
                                select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, t.tags.Select(x => new string(x.Name)).ToArray(), t.State);
        return new ReadOnlyCollection<TaskDTO>(taskCollection.ToList());
    }
    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        throw new NotImplementedException();
    }
    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        var taskCollection =    from t in _context.Tasks
                                where t.AssignedTo.Id == userId
                                select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, t.tags.Select(x => new string(x.Name)).ToArray(), t.State);
        return new ReadOnlyCollection<TaskDTO>(taskCollection.ToList());
    }
    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        var taskCollection =    from t in _context.Tasks
                                where t.State == state
                                select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, t.tags.Select(x => new string(x.Name)).ToArray(), t.State);
        return new ReadOnlyCollection<TaskDTO>(taskCollection.ToList());
    }
    public TaskDetailsDTO Read(int taskId)
    {
        var tasks = from t in _context.Tasks
                    where t.Id == taskId
                    select new TaskDetailsDTO(t.Id, t.Title, t.Description, t.Created, t.AssignedTo.Name, t.tags.Select(x => new string(x.Name)).ToArray(), t.State, t.StateUpdated);

        return tasks.FirstOrDefault();
    }
    public Response Update(TaskUpdateDTO task)
    {
        var entity = _context.Tasks.Find(task.Id);
        Response response;

        if (entity is null)
        {
            response = Response.NotFound;
        }
        else if (_context.Tasks.FirstOrDefault(t => t.Id != entity.Id && t.Title == entity.Title) != null)
        {
            response = Response.Conflict;
        }
        else
        {
            entity.Title = task.Title;
            entity.AssignedTo = _context.Users.Find(task.AssignedToId);
            entity.Description = task.Description;
            ICollection<Tag> tagsList = new List<Tag>();
            foreach (var s in task.Tags)
            {
                var tagsQuery = _context.Tags.Where(t => t.Name == s).Select(t => t);
                foreach (var t in tagsQuery)
                {
                    tagsList.Add(t);
                }
            }
            entity.tags = tagsList;
            if (entity.State != task.State)
            {
                entity.StateUpdated = DateTime.Now;
                entity.State = task.State;
            }
            if (entity.AssignedTo == null)
            {
                return Response.BadRequest;
            }
            _context.SaveChanges();
            response = Response.Updated;
        }

        return response;
    }
    public Response Delete(int taskId)
    {
        var task = _context.Tasks.Include(t => t.tags).FirstOrDefault(t => t.Id == taskId);
        Response response;

        if (task is null)
        {
            response = Response.NotFound;
        }
        else if (task.State == State.Resolved || task.State == State.Closed || task.State == State.Removed)
        {
            response = Response.Conflict;
        }
        else
        {
            if (task.State == State.Active)
            {
                task.State = State.Removed;
            }
            else
            {
                _context.Tasks.Remove(task);
            }
            _context.SaveChanges();

            response = Response.Deleted;
        }

        return response;
    }
}
