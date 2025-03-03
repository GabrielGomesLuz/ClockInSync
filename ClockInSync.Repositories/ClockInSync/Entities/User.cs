﻿using ClockInSync.Repositories.Entities.Enums;
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
        [StringLength(254)]
        public string Password { get; set; } = string.Empty; // Hash da senha

        [Required]
        public Role Role { get; set; }

        [Required]
        [StringLength(30)]
        public string Position { get; set; } = string.Empty;

        [Required]
        [StringLength(40)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Level { get; set; } = string.Empty;

        // Chave estrangeira para a tabela Settings

        public Guid SettingsId { get; set; }

        // Navegação para as configurações do usuário
        [ForeignKey("SettingsId")]
        public Settings Settings { get; set; }
    }
}
