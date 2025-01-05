using AutoMapper;
using ClockInSync.Repositories.Dtos.PunchClock;
using ClockInSync.Repositories.Dtos.User;
using ClockInSync.Repositories.Dtos.User.UserResponse;
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

        CreateMap<UserLoginDto, User>();

        CreateMap<RegisterPunchClock, PunchClock>();

        CreateMap<UserLoginInformationResponse, User>();
    }
}
