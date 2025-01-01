using System.ComponentModel.DataAnnotations;

namespace ClockInSync.Repositories.Dtos.Settings
{
    public class SettingsDto
    {

        [Required]
        public decimal WorkdayHours { get; set; } = 8.0m;

        [Required]
        public decimal OvertimeRate { get; set; } = 0.0m;


    }
}
