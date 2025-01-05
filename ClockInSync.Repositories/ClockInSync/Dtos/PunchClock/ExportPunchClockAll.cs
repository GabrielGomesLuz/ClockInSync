namespace ClockInSync.Repositories.Dtos.PunchClock
{
    public class ExportPunchClockAll
    {
        public List<Employees> Employees { get; set; } = [];

    }

    public class Employees
    {
        public string Name { get; set; } = string.Empty;

        public string HoursWorked { get; set; } = string.Empty;

        public int WeeklyJourney { get; set; } 
    }
}
