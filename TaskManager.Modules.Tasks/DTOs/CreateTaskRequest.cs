namespace TaskManager.Modules.Tasks.DTOs;

public class CreateTaskRequest
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? DueDate { get; set; }
    public string Priority { get; set; } = "Medium";
}
