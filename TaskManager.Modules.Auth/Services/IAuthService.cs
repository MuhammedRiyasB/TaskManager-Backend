using TaskManager.Modules.Auth.Contracts;

namespace TaskManager.Modules.Auth.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
