using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClockInSync.Repositories.Entities
{
    public class PunchClock 
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }  // Referência ao usuário

        [Required]
        public PunchType Type { get; set; }  // Tipo de ação (check-in ou check-out)

        [Required]
        public DateTime Timestamp { get; set; }  // Data e hora do punch

        // Relacionamento com o usuário
        [ForeignKey("UserId")]
        public User User { get; set; }


        public enum PunchType
        {
            CheckIn,
            CheckOut
        }

    }
}
