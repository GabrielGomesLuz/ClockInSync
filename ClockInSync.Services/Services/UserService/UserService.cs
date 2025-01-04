using AutoMapper;
using ClockInSync.Repositories.ClockInSync.Dtos.User.UserResponse;
using ClockInSync.Repositories.ClockInSync.Entities.Enums;
using ClockInSync.Repositories.Dtos.User;
using ClockInSync.Repositories.Dtos.User.UserResponse;
using ClockInSync.Repositories.Entities;
using ClockInSync.Repositories.Repositories;
using ClockInSync.Services.PasswordManagementService;
using ClockInSync.Services.TokenServices;

namespace ClockInSync.Services
{

    public interface IUserService
    {
        Task<UserRegisterResponse> CreateUserAsync(UserCreationDto user);

        Task<UserLoginResponse?> LoginUserAsync(UserLoginDto user);

        Task<bool> VerifyUserExistsByEmailAsync(string email);
    }


    public class UserService : IUserService
    {

        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;

        public UserService(IUserRepository userRepository, IMapper mapper, ITokenService tokenService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }

        public async Task<UserRegisterResponse> CreateUserAsync(UserCreationDto userCreation)
        {
            var user = mapper.Map<User>(userCreation);
            user.Id = Guid.NewGuid();
            user.Settings.Id = Guid.NewGuid();
            // Cria o hash da senha
            user.Password = PasswordHashHelper.HashPassword(user.Password, user);

            return await userRepository.CreateUserAsync(user);
        }

        public async Task<UserLoginResponse?> LoginUserAsync(UserLoginDto userLogin)
        {
            var user = mapper.Map<User>(userLogin);
            var userFound = await userRepository.VerifyUserLoginAsync(user);

            if (userFound != null)
            {
                var token = tokenService.GenerateToken(user);
                return new UserLoginResponse { JwtToken = token, Role = userFound.Role };

            }
            return null;
        }

        public async Task<bool> VerifyUserExistsByEmailAsync(string email)
        {
            return await userRepository.VerifyUserExistsByEmailAsync(email);
        }
    }
}
