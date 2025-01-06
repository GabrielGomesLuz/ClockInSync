using ClockInSync.Repositories.Dtos.Settings;
using ClockInSync.Repositories.Entities.Enums;

namespace ClockInSync.Repositories.Dtos.User.UserResponse
{
    public class UserAllDetailsResponse : UserInformationResponse
    {

        public string HoursWorked { get; set; } = string.Empty;

        public Role Role { get; set; }

        public List<Registers> Registers { get; set; } = [];

    }

    public sealed class Registers
    {

        public List<PunchDetail> CheckIns { get; set; } = [];
        public List<PunchDetail> CheckOuts { get; set; } = [];
        public DateTime Date { get; set; }

        public string Message {  get; set; } = string.Empty;
    }

    public class PunchDetail
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
