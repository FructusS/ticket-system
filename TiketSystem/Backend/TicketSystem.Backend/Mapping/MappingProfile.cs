using AutoMapper;
using Shared.ViewModels;
using TicketSystem.Database.Models;
using TicketSystem.Shared.ViewModels;
using Task = TicketSystem.Database.Models.Task;

namespace TicketSystem.Backend.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.UserName));
              //  .ForMember(x => x.UserRole, opt => opt.MapFrom(x => x.UserRole.Title));

            CreateMap<UserViewModel, User>();


            CreateMap<Task, TaskViewModel>()
                .ForMember(x => x.TaskStatus, opt => opt.MapFrom(x => x.TaskStatus))
                .ForMember(x => x.User, opt => opt.MapFrom(x => x.User))
                .ForMember(x => x.Cabinet, opt => opt.MapFrom(x => x.Cabinet))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description));

            CreateMap<TaskViewModel, Task>()
                .ForMember(x => x.TaskStatus, opt => opt.MapFrom(x => x.TaskStatus))
                .ForMember(x => x.User, opt => opt.MapFrom(x => x.User))
                .ForMember(x => x.Cabinet, opt => opt.MapFrom(x => x.Cabinet))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description));
        }
    }
}
