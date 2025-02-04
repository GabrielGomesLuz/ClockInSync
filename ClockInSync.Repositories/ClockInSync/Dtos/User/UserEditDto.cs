using ClockInSync.Repositories.Dtos.Settings;

namespace ClockInSync.Repositories.Dtos.User
{
    public class UserEditDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string Level {get; set;} = string.Empty;

        public SettingsDto Settings { get; set; } = new();

    }
}
