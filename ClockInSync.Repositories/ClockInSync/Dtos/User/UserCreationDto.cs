using ClockInSync.Repositories.ClockInSync.Entities.Enums;
using ClockInSync.Repositories.Dtos.Settings;
using System.ComponentModel.DataAnnotations;

namespace ClockInSync.Repositories.Dtos.User
{
    public class UserCreationDto
    {
        [Required]
        [StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(254)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(64)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; }

        [Required]
        public SettingsDto Settings { get; set; }


    }
}
