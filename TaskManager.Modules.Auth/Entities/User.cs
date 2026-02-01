namespace TaskManager.Modules.Auth.Entities;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
