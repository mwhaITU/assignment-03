namespace Assignment3.Entities;

public class User
{
    int Id { get; set; }

    [StringLength(100), Required]
    string? Name { get; set; }

    [StringLength(100), Required]
    string? Email { get; set; }

    List<Task>? Tasks { get; set; }
}
