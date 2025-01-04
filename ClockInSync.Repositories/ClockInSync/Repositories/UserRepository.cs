using ClockInSync.Repositories.ClockInSync.Dtos.User.UserResponse;
using ClockInSync.Repositories.DbContexts;
using ClockInSync.Repositories.Dtos.User;
using ClockInSync.Repositories.Dtos.User.UserResponse;
using ClockInSync.Repositories.Entities;
using ClockInSync.Repositories.PasswordManagementHelper;
using Microsoft.EntityFrameworkCore;

namespace ClockInSync.Repositories.Repositories
{

    public interface IUserRepository
    {
        Task<UserRegisterResponse> CreateUserAsync(User user);

        Task<UserInformationResponse?> UpdateUserAsync(User user);


        Task<bool> DeleteUserAsync(Guid userId);

        Task<UserLoginInformationResponse?> GetUserByEmailAsync(string email);

        Task<IEnumerable<UserInformationResponse>> GetUsersInformationAsync();

        Task<UserLoginInformationResponse?> VerifyUserLoginAsync(User login);

        Task<bool> VerifyUserExistsByEmailAsync(string email);
    }

    public class UserRepository(ClockInSyncDbContext dbContext) : IUserRepository
    {
        public readonly ClockInSyncDbContext dbContext = dbContext;

        public async Task<UserRegisterResponse> CreateUserAsync(User user)
        {
            dbContext.Settings.Add(user.Settings);
            await dbContext.SaveChangesAsync();

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return new UserRegisterResponse { Id = user.Id, Message = "User created successfully" };
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var userFound = await dbContext.Users.FindAsync(userId);
            if (userFound != null)
            {
                dbContext.Users.Remove(userFound);
                await dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<UserLoginInformationResponse?> GetUserByEmailAsync(string email)
        {
            var userFound = await dbContext.Users.Where(p => p.Email == email).FirstOrDefaultAsync();

            if (userFound != null)
            {
                return new UserLoginInformationResponse
                {
                    Id = userFound.Id,
                    Email = email,
                    Name = userFound.Name,
                    Password = userFound.Password,
                    Role = userFound.Role,
                };
            }

            return null;
        }

        public async Task<IEnumerable<UserInformationResponse>> GetUsersInformationAsync()
        {
            return await dbContext.Users
        .Select(u => new UserInformationResponse
        {
            Id = u.Id,
            Email = u.Email,
            Name = u.Name,
            Settings = new Dtos.Settings.SettingsDto { OvertimeRate = u.Settings.OvertimeRate, WorkdayHours = u.Settings.WorkdayHours }
        })
        .ToListAsync();
        }

        public async Task<UserInformationResponse?> UpdateUserAsync(User user)
        {
            if (user != null)
            {
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();
                return new UserInformationResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name
                };
            }
            return null;
        }

        public async Task<UserLoginInformationResponse?> VerifyUserLoginAsync(User login)
        {

            var userFound = await GetUserByEmailAsync(login.Email);

            if (userFound != null)
            {
                
                var matchPassword = PasswordHashHelper.VerifyPassword(userFound.Password,login.Password,login);
                if (matchPassword)
                    return new UserLoginInformationResponse { Id = userFound.Id, Email = userFound.Email, Name = userFound.Name,Role = userFound.Role};

            }
            return null;
        }

        public async Task<bool> VerifyUserExistsByEmailAsync(string email)
        {
            return await dbContext.Users.AnyAsync(p => p.Email == email);
        }
    }
}
