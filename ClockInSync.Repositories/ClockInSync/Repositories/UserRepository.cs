using ClockInSync.Repositories.ClockInSync.Dtos.User.UserResponse;
using ClockInSync.Repositories.DbContexts;
using ClockInSync.Repositories.Dtos.Settings;
using ClockInSync.Repositories.Dtos.User;
using ClockInSync.Repositories.Dtos.User.UserResponse;
using ClockInSync.Repositories.Entities;
using ClockInSync.Repositories.PasswordManagementHelper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using static ClockInSync.Repositories.Entities.PunchClock;

namespace ClockInSync.Repositories.Repositories
{

    public interface IUserRepository
    {
        Task<UserRegisterResponse> CreateUserAsync(User user);

        Task<string?> UpdateUserAsync(User user);


        Task<bool> DeleteUserAsync(Guid userId);

        Task<UserLoginInformationResponse?> GetUserByEmailAsync(string email);

        Task<IEnumerable<UserInformationResponse>> GetUsersInformationAsync(int offset, int limit);

        Task<UserLoginInformationResponse?> VerifyUserLoginAsync(User login);

        Task<bool> VerifyUserExistsByEmailAsync(string email);

        public Task<UserAllDetailsResponse?> GetUserAllDetails(Guid userId);

        public Task<UserInfoToEditResponse?> GetUserInfoToEditAsync(Guid userId);

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

        public async Task<IEnumerable<UserInformationResponse>> GetUsersInformationAsync(int offset, int limit)
        {
            return await dbContext.Users
        .Skip(offset)
        .Select(u => new UserInformationResponse
        {
            Id = u.Id,
            Email = u.Email,
            Name = u.Name,
            Department = u.Department,
            Position = u.Position,
            Level = u.Level,
            Settings = new Dtos.Settings.SettingsDto
            {
                OvertimeRate = u.Settings.OvertimeRate,
                WorkdayHours = u.Settings.WorkdayHours,
                WeeklyJourney = u.Settings.WeeklyJourney,
            }
        })
        .ToListAsync();


        }

        public async Task<string?> UpdateUserAsync(User user)
        {

            var userFound = await VerifyUserExistsById(user.Id);
            if (userFound)
            {
                var userOld = await GetUserCredentialsByIdAsync(user.Id);
                user.Password = userOld.UserPassword;
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();
                return "Dados atualizados com sucesso.";
            }
            return "Usuário não existe.";
        }


        private async Task<UserCredentialsResponse> GetUserCredentialsByIdAsync(Guid userId)
        {
            var userData = await dbContext.Users.Where(u => u.Id == userId).Select(u => new UserCredentialsResponse
            {
                UserName = u.Name,
                UserPassword = u.Password,
            }).FirstOrDefaultAsync();

            return userData;
        }
        private async Task<bool> VerifyUserExistsById(Guid userId)
        {

            return await dbContext.Users.AnyAsync(u => u.Id == userId);

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

        public async Task<UserAllDetailsResponse?> GetUserAllDetails(Guid userId)
        {
            var userFound = await dbContext.Users.AnyAsync(u => u.Id == userId);

            if (!userFound)
                return null;

            var userDetails = await dbContext.Users
    .Where(u => u.Id == userId)
    .Select(u => new UserAllDetailsResponse
    {
        Id = u.Id,
        Name = u.Name,
        Email = u.Email,
        Role = u.Role,
        Department = u.Department,
        Position = u.Position,
        Level = u.Level,
        Settings = new SettingsDto
        {
            WorkdayHours = u.Settings.WorkdayHours,
            OvertimeRate = u.Settings.OvertimeRate,
            WeeklyJourney = u.Settings.WeeklyJourney
        },
        Registers = dbContext.PunchClocks
            .Where(p => p.UserId == u.Id)
            .GroupBy(p => new { p.UserId, p.Timestamp.Date })
            .Select(g => new Registers
            {
                Date = g.Key.Date,
                CheckIns = g.Where(x => x.Type == PunchType.CheckIn)
                            .Select(x => new PunchDetail { Timestamp = x.Timestamp, Message = x.Message })
                            .ToList(),
                CheckOuts = g.Where(x => x.Type == PunchType.CheckOut)
                             .Select(x => new PunchDetail { Timestamp = x.Timestamp, Message = x.Message })
                             .ToList(),
            })
            .ToList()
    })
    .FirstOrDefaultAsync();

            if (userDetails != null)
            {
                decimal totalHoursWorked = 0;
                foreach (var register in userDetails.Registers)
                {
                    foreach (var checkIn in register.CheckIns)
                    {
                        var checkOut = register.CheckOuts.FirstOrDefault(co => co.Timestamp.Date == checkIn.Timestamp.Date);
                        if (checkOut != null)
                        {
                            totalHoursWorked += (decimal)(checkOut.Timestamp - checkIn.Timestamp).TotalHours;
                        }
                    }
                }
                userDetails.HoursWorked = totalHoursWorked.ToString("F2");
            }

            return userDetails;


        }

        public async Task<UserInfoToEditResponse?> GetUserInfoToEditAsync(Guid userId)
        {
            var userFound = await dbContext.Users.AnyAsync(u => u.Id == userId);

            if (!userFound)
                return null;

            var userEditDetails = await dbContext.Users
    .Where(u => u.Id == userId)
    .Select(u => new UserInfoToEditResponse
    {
        Id = u.Id,
        Name = u.Name,
        Email = u.Email,
        Department = u.Department,
        Position = u.Position,
        Role = u.Role,
        Level = u.Level,
        Settings = new SettingsDto
        {
            WorkdayHours = u.Settings.WorkdayHours,
            OvertimeRate = u.Settings.OvertimeRate,
            WeeklyJourney = u.Settings.WeeklyJourney
        },
    })
    .FirstOrDefaultAsync();

            return userEditDetails;


        }

    }
}
