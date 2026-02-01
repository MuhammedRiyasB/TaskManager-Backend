namespace TaskManager.Modules.Tasks.Models;

public class TaskItem
{
    public int Id { get; set; }

    public int UserId { get; set; } = default!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    public DateTime? DueDate { get; set; }

    public string Priority { get; set; } = "Medium";

    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
