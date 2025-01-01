using AutoMapper;
using ClockInSync.Repositories.ClockInSync.Dtos.User;
using ClockInSync.Repositories.ClockInSync.Dtos.User.UserResponse;
using ClockInSync.Repositories.Entities;
using ClockInSync.Repositories.Repositories;
using ClockInSync.Services.PasswordManagementService;

namespace ClockInSync.Services
{

    public interface IUserService
    {
        Task<UserRegisterResponse> CreateUserAsync(UserCreationDto user);
    }


    public class UserService : IUserService
    {

        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IPasswordService _passwordService;

        public UserService(IUserRepository userRepository,IMapper mapper,IPasswordService _passwordService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this._passwordService = _passwordService;
        }

        public async Task<UserRegisterResponse> CreateUserAsync(UserCreationDto userCreation)
        {
            var user = mapper.Map<User>(userCreation);
            user.Id = Guid.NewGuid();
            user.Settings.Id = Guid.NewGuid();
            // Cria o hash da senha
            user.Password = _passwordService.HashPassword(user.Password,user.Id);

            bool tst = _passwordService.VerifyPassword(user.Password,userCreation.Password,user.Id);
            return await userRepository.CreateUserAsync(user);
        }


    }
}
