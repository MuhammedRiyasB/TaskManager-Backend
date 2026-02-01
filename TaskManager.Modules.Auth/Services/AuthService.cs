using AutoMapper;
using TaskManager.Modules.Auth.Contracts;
using TaskManager.Modules.Auth.Entities;
using TaskManager.Modules.Auth.Jwt;
using TaskManager.Modules.Auth.Repositories;
using TaskManager.Modules.Auth.Security;

namespace TaskManager.Modules.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwt;
    private readonly IMapper _mapper;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwt,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
        _mapper = mapper;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.EmailExistsAsync(request.Email))
            throw new ApplicationException("Email already registered");

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password)
        };

        await _userRepository.AddAsync(user);

        var token = _jwt.Generate(user);
        var response = _mapper.Map<AuthResponse>(user);
        response.Token = token;

        return response;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new ApplicationException("Invalid email or password");

        var token = _jwt.Generate(user);
        var response = _mapper.Map<AuthResponse>(user);
        response.Token = token;

        return response;
    }
}
