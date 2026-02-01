namespace TaskManager.Modules.Auth.Contracts;

public class AuthResponse
{
    public int UserId { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;
}
