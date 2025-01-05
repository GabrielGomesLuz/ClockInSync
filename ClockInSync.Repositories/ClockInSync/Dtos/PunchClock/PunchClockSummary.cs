namespace ClockInSync.Repositories.Dtos.PunchClock
{
    public class PunchClockSummary
    {
        public string Date { get; set; } = string.Empty;
        public string CheckIn { get; set; } = string.Empty;
        public string CheckOut { get; set; } = string.Empty;
        public string HoursWorked { get; set; } = string.Empty ;
    }
}
