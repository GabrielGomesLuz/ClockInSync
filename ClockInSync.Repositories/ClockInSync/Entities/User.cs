using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClockInSync.Repositories.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(254)]
        public string Email { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; } = string.Empty; // Hash da senha

        [Required]
        [StringLength(64)]
        public string PasswordSalt { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; }

        // Chave estrangeira para a tabela Settings
        
        public Guid SettingsId { get; set; }

        // Navegação para as configurações do usuário
        [ForeignKey("SettingsId")]
        public Settings Settings { get; set; }
    }

    public enum Role
    {
        Employee,
        Admin
    }
}
