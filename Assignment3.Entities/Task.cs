namespace Assignment3.Entities;
using Assignment3.Core;

public class Task
{
    public int Id { get; set; }

    [StringLength(100), Required]
    public string? Title { get; set; }

    public User? AssignedTo { get; set; }

    [StringLength(int.MaxValue)]
    public string? Description { get; set; }

    [Required]
    public State State { get; set; }

    public ICollection<Tag>? tags { get; set; }
}