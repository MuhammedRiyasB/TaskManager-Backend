using AutoMapper;
using TaskManager.Modules.Tasks.DTOs;
using TaskManager.Modules.Tasks.Models;

namespace TaskManager.Modules.Tasks.Mapping;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskItem, TaskResponse>();

        CreateMap<CreateTaskRequest, TaskItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsCompleted, opt => opt.Ignore());

        CreateMap<UpdateTaskRequest, TaskItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}
