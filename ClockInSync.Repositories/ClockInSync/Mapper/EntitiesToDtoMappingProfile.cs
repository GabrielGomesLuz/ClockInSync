using AutoMapper;
using ClockInSync.Repositories.ClockInSync.Dtos.User;
using ClockInSync.Repositories.Entities;

namespace ClockInSync.Repositories.ClockInSync.Mapper;
public class EntitiesToDtoMappingProfile : Profile
{
    public EntitiesToDtoMappingProfile()
    {
        CreateMap<UserCreationDto, User>()
       .ForMember(dest => dest.Settings, opt => opt.MapFrom(src => new Settings
       {
           WorkdayHours = src.Settings.WorkdayHours,
           OvertimeRate = src.Settings.OvertimeRate
       }));
    }
}
