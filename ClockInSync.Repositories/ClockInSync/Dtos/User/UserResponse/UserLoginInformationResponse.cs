using ClockInSync.Repositories.Entities.Enums;

namespace ClockInSync.Repositories.Dtos.User.UserResponse
{
    public class UserLoginInformationResponse 
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; }
    }
}
