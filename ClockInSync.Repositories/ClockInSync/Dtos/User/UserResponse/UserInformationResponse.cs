using ClockInSync.Repositories.Dtos.Settings;

namespace ClockInSync.Repositories.Dtos.User.UserResponse
{
    public class UserInformationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public SettingsDto Settings { get; set; }


    }
}
