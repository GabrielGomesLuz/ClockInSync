using System.ComponentModel.DataAnnotations;

namespace ClockInSync.Repositories.Entities
{
    public class Settings
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public decimal WorkdayHours { get; set; }  // Horas padrão por dia

        public decimal OvertimeRate { get; set; }

        public int WeeklyJourney { get; set; }


        // Navegação para o usuário
        public User User { get; set; }
    }
}
