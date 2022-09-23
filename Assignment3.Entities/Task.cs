namespace Assignment3.Entities;


public class Task
{
    int Id { get; set; }

    [StringLength(100), Required]
    string? Title { get; set; }

    User? AssignedTo { get; set; }

    [StringLength(int.MaxValue)]
    string? Description { get; set; }

    [Required]
    string? State { get; set; }

    ICollection<Tag>? tags { get; set; }
}
