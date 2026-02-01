using AutoMapper;
using TaskManager.Modules.Auth.Contracts;
using TaskManager.Modules.Auth.Entities;

namespace TaskManager.Modules.Auth.Mapping;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<User, AuthResponse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Token, opt => opt.Ignore());
    }
}
