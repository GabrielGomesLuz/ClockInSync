using ClockInSync.Repositories.Dtos.Settings;
using ClockInSync.Repositories.Entities.Enums;
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
        [StringLength(40)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Position {  get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Level {  get; set; }

        [Required]
        public SettingsDto Settings { get; set; }


    }
}
