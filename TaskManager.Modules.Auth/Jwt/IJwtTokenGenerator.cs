using TaskManager.Modules.Auth.Entities;

namespace TaskManager.Modules.Auth.Jwt;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}
