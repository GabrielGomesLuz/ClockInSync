namespace ClockInSync.Repositories.ClockInSync.Dtos.User.UserResponse
{
    public class UserInformationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

    }
}
