namespace Assignment3.Entities;

public class Tag
{
    int Id { get; set; }

    [StringLength(50), Required]
    string? Name { get; set; }

    ICollection<Task>? Tasks { get; set; }
}
