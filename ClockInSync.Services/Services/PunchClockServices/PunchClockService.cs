using AutoMapper;
using ClockInSync.Repositories.ClockInSync.Repositories;
using ClockInSync.Repositories.Dtos.PunchClock;
using ClockInSync.Repositories.Entities;
using System.Text;

namespace ClockInSync.Services.PunchClockServices
{

    public interface IPunchClockService
    {
        Task<bool> RegisterPunchClock(RegisterPunchClock registerPunchClock, Guid userId);
        Task<IEnumerable<PunchClockSummary>> GetPunchClockSummaries(Guid userId);
        public Task<IEnumerable<PunchClockAll>> GetPunchClockAll(Guid? userId, DateTime? startDate, DateTime? endDate);
        public Task<byte[]> ExportPunchClockAll(DateTime? startDate, DateTime? endDate);
    }

    public class PunchClockService(IMapper mapper, IPunchClockRepository punchClockRepository) : IPunchClockService
    {
        public async Task<byte[]> ExportPunchClockAll(DateTime? startDate, DateTime? endDate)
        {
            var data = await punchClockRepository.ExportAllPunchClocks(startDate, endDate);
            return GenerateCsv(data);
        }

        public byte[] GenerateCsv(ExportPunchClockAll exportData)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Name,HoursWorked,WeeklyJourney");

            foreach (var employee in exportData.Employees)
            {
                csvBuilder.AppendLine($"{employee.Name},{employee.HoursWorked},{employee.WeeklyJourney}");
            }

            // Converter para byte array
            return Encoding.UTF8.GetBytes(csvBuilder.ToString());
        }

        public async Task<IEnumerable<PunchClockAll>> GetPunchClockAll(Guid? userId, DateTime? startDate, DateTime? endDate)
        {
            return await punchClockRepository.GetAllPunchClock(userId, startDate, endDate);
        }

        public async Task<IEnumerable<PunchClockSummary>> GetPunchClockSummaries(Guid userId)
        {
            return await punchClockRepository.GetPunchClockSummaries(userId);
        }

        public async Task<bool> RegisterPunchClock(RegisterPunchClock registerPunchClock, Guid userId)
        {
            var punchClock = mapper.Map<PunchClock>(registerPunchClock);

            punchClock.UserId = userId;
            punchClock.Id = Guid.NewGuid();
            punchClock.Timestamp = DateTime.Now;

            return await punchClockRepository.RegisterPunchClockAsync(punchClock);
        }
    }
}
