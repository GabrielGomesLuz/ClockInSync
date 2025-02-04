using AutoMapper;
using ClockInSync.Repositories.ClockInSync.Dtos.User.UserResponse;
using ClockInSync.Repositories.Dtos.User;
using ClockInSync.Repositories.Dtos.User.UserResponse;
using ClockInSync.Repositories.Entities;
using ClockInSync.Repositories.PasswordManagementHelper;
using ClockInSync.Repositories.Repositories;
using ClockInSync.Services.TokenServices;

namespace ClockInSync.Services
{

    public interface IUserService
    {
        Task<UserRegisterResponse> CreateUserAsync(UserCreationDto user);

        Task<UserLoginResponse?> LoginUserAsync(UserLoginDto user);

        Task<bool> VerifyUserExistsByEmailAsync(string email);

        Task<IEnumerable<UserInformationResponse>> GetUsersInformationAsync(int offset, int limit);

         Task<UserAllDetailsResponse?> GetUserAllDetails(Guid userId);

        Task<UserInformationResponse?> GetUserInformation(Guid userId);


        public Task<string?> UpdateUserAsync(UserEditDto userEditDto);

        public Task<UserInfoToEditResponse?> GetUserInfoToEditAsync(Guid userId);
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

        public async Task<UserAllDetailsResponse?> GetUserAllDetails(Guid userId)
        {
            return await userRepository.GetUserAllDetails(userId);
        }

        public async Task<UserInformationResponse?> GetUserInformation(Guid userId)
        {
            return await userRepository.GetUserBasicInfoAsync(userId);
        }

        public async Task<UserInfoToEditResponse?> GetUserInfoToEditAsync(Guid userId)
        {
            return await userRepository.GetUserInfoToEditAsync(userId);
        }

        public async Task<IEnumerable<UserInformationResponse>> GetUsersInformationAsync(int offset, int limit)
        {
            return await userRepository.GetUsersInformationAsync(offset, limit);
        }

        public async Task<UserLoginResponse?> LoginUserAsync(UserLoginDto userLogin)
        {
            var user = mapper.Map<User>(userLogin);
            var userFound = await userRepository.VerifyUserLoginAsync(user);

            if (userFound != null)
            {
                var u = mapper.Map<User>(userFound);
                var token = tokenService.GenerateToken(u);
                return new UserLoginResponse { JwtToken = token, Role = userFound.Role , Message = "Login realizado com sucesso."};

            }
            return null;
        }

        public async Task<string?> UpdateUserAsync(UserEditDto userEditDto)
        {
            var user = mapper.Map<User>(userEditDto);

            return await userRepository.UpdateUserAsync(user);

        }

        public async Task<bool> VerifyUserExistsByEmailAsync(string email)
        {
            return await userRepository.VerifyUserExistsByEmailAsync(email);
        }
    }
}
