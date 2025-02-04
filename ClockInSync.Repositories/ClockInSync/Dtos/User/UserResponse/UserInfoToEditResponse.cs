using ClockInSync.Repositories.Entities.Enums;

namespace ClockInSync.Repositories.Dtos.User.UserResponse
{
    public class UserInfoToEditResponse : UserInformationResponse
    {
        public string HoursWorked { get; set; } = string.Empty;

        public Role Role { get; set; }

    }
}
