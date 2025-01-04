using ClockInSync.Repositories.ClockInSync.Entities.Enums;

namespace ClockInSync.Repositories.Dtos.User.UserResponse
{
    public class UserLoginResponse
    {

        public string JwtToken { get; set; } = string.Empty;
        public Role Role { get; set; } 


    }
}
