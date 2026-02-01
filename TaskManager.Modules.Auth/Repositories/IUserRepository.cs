using TaskManager.Modules.Auth.Entities;

namespace TaskManager.Modules.Auth.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task<bool> EmailExistsAsync(string email);
}
