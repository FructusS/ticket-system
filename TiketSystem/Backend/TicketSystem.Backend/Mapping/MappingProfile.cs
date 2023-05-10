using AutoMapper;
using TicketSystem.Database.Models;
using TicketSystem.Shared.ViewModels;

namespace TicketSystem.Backend.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(x => x.UserName, opt => opt.MapFrom((x => x.UserName)))
                .ForMember(x=> x.UserRole, opt => opt.MapFrom(x=> x.UserRole.Title));

            CreateMap<UserViewModel, User>();


        }
    }
}
