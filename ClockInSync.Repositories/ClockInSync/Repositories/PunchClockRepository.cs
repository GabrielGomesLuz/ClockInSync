using ClockInSync.Repositories.DbContexts;
using ClockInSync.Repositories.Dtos.PunchClock;
using ClockInSync.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static ClockInSync.Repositories.Entities.PunchClock;

namespace ClockInSync.Repositories.ClockInSync.Repositories
{

    public interface IPunchClockRepository
    {
        Task<bool> RegisterPunchClockAsync(PunchClock punchClock);

        public Task<IEnumerable<PunchClockSummary>> GetPunchClockSummaries(Guid userId);

        public Task<IEnumerable<PunchClockAll>> GetAllPunchClock(Guid? userId,DateTime? startDate, DateTime? endDate);

        public Task<ExportPunchClockAll> ExportAllPunchClocks(DateTime? startDate, DateTime? endDate);

    }

    public class PunchClockRepository(ClockInSyncDbContext dbContext) : IPunchClockRepository
    {
        public readonly ClockInSyncDbContext dbContext = dbContext;

        public async Task<IEnumerable<PunchClockSummary>> GetPunchClockSummaries(Guid userId)
        {
            var punchClockData = await dbContext.PunchClocks
       .Where(pc => pc.UserId == userId)
       .GroupBy(pc => pc.Timestamp.Date)
       .Select(group => new
       {
           Date = group.Key,
           CheckIn = group.Where(x => x.Type == PunchType.CheckIn)
                          .Select(x => x.Timestamp)
                          .FirstOrDefault(),
           CheckOut = group.Where(x => x.Type == PunchType.CheckOut)
                           .Select(x => x.Timestamp)
                           .FirstOrDefault()
       })
       .ToListAsync();

            var punchClockSummaries = punchClockData
        .Select(x => new PunchClockSummary
        {
            Date = x.Date.ToString("yyyy-MM-dd"),
            CheckIn = x.CheckIn.ToString("HH:mm") ?? "Sem CheckIn realizado.",
            CheckOut = x.CheckOut.ToString("HH:mm") ?? "Sem CheckOut realizado.",
            HoursWorked = CalculateHoursWorked(x.CheckIn, x.CheckOut).ToString("F2", CultureInfo.InvariantCulture) ?? "0.00"
        })
        .ToList();

            return punchClockSummaries;
        }

        public async Task<bool> RegisterPunchClockAsync(PunchClock punchClock)
        {
            dbContext.PunchClocks.Add(punchClock);
            var isRegister = await dbContext.SaveChangesAsync();
            return isRegister > 0;
        }

        private static double CalculateHoursWorked(DateTime? checkIn, DateTime? checkOut)
        {
            if (checkIn.HasValue && checkOut.HasValue)
            {
                var totalMinutes = (checkOut.Value - checkIn.Value).TotalMinutes;
                return Math.Round(totalMinutes / 60, 2);

            }
            return 0;
        }

        public async Task<IEnumerable<PunchClockAll>> GetAllPunchClock(Guid? userId, DateTime? startDate, DateTime? endDate)
        {
            var query = dbContext.PunchClocks
        .Join(dbContext.Users, // Relacionar com a tabela de usuários
              pc => pc.UserId,
              user => user.Id,
              (pc, user) => new
              {
                  pc.Timestamp,
                  pc.Type,
                  pc.UserId,
                  UserName = user.Name // Nome do funcionário
              })
        .AsQueryable();

            // Filtrar por userId se fornecido
            if (userId != Guid.Empty && userId != null)
            {
                query = query.Where(pc => pc.UserId == userId.Value);
            }

            // Filtrar por intervalo de datas se fornecido
            if (startDate.HasValue && startDate != default(DateTime))
            {
                query = query.Where(pc => pc.Timestamp.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue && endDate != default(DateTime))
            {
                query = query.Where(pc => pc.Timestamp.Date <= endDate.Value.Date);
            }

            var punchClockData = await query
                .GroupBy(pc => new { pc.Timestamp.Date, pc.UserId, pc.UserName }) // Agrupar por data, usuário e nome
                .Select(group => new
                {
                    Date = group.Key.Date,
                    UserName = group.Key.UserName,
                    CheckIn = group.Where(x => x.Type == PunchType.CheckIn)
                                   .Select(x => x.Timestamp)
                                   .FirstOrDefault(),
                    CheckOut = group.Where(x => x.Type == PunchType.CheckOut)
                                    .Select(x => x.Timestamp)
                                    .FirstOrDefault()
                })
                .ToListAsync();

            var punchClockSummaries = punchClockData
                .Select(x => new PunchClockAll
                {
                    Date = x.Date.ToString("yyyy-MM-dd"),
                    Name = x.UserName,
                    CheckIn = x.CheckIn != default ? x.CheckIn.ToString("HH:mm") : "Sem CheckIn realizado.",
                    CheckOut = x.CheckOut != default ? x.CheckOut.ToString("HH:mm") : "Sem CheckOut realizado.",
                    HoursWorked = x.CheckIn != default && x.CheckOut != default
                        ? CalculateHoursWorked(x.CheckIn, x.CheckOut).ToString("F2", CultureInfo.InvariantCulture)
                        : "0.00"
                })
                .ToList();

            return punchClockSummaries;
        }

        public async Task<ExportPunchClockAll> ExportAllPunchClocks(DateTime? startDate, DateTime? endDate)
        {

            var query = dbContext.PunchClocks
    .Join(dbContext.Users,
          pc => pc.UserId,
          user => user.Id,
          (pc, user) => new
          {
              pc.Timestamp,
              pc.Type,
              pc.UserId,
              UserName = user.Name,
              SettingsId = user.SettingsId
          })
    .Join(dbContext.Settings,
          pc => pc.SettingsId,
          settings => settings.Id,
          (pc, settings) => new
          {
              pc.Timestamp,
              pc.Type,
              pc.UserId,
              pc.UserName,
              WeeklyJourney = settings.WeeklyJourney // Jornada semanal do Settings
          })
    .AsQueryable();


            // Filtrar por intervalo de datas se fornecido
            if (startDate.HasValue && startDate != default(DateTime))
            {
                query = query.Where(pc => pc.Timestamp.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue && endDate != default(DateTime))
            {
                query = query.Where(pc => pc.Timestamp.Date <= endDate.Value.Date);
            }





            var punchClockData = await query
                .GroupBy(pc => new { pc.UserId, pc.UserName, pc.WeeklyJourney }) // Agrupar por data, usuário e nome
                .Select(group => new
                {
                    UserName = group.Key.UserName,
                    CheckIn = group.Where(x => x.Type == PunchType.CheckIn)
                                   .Select(x => x.Timestamp)
                                   .FirstOrDefault(),
                    CheckOut = group.Where(x => x.Type == PunchType.CheckOut)
                                    .Select(x => x.Timestamp)
                                    .FirstOrDefault(),

                    WeeklyJourney = group.Key.WeeklyJourney,
                })
                .ToListAsync();

            var punchClockSummaries = punchClockData
                .Select(x => new Employees
                {
                    Name = x.UserName,
                    HoursWorked = x.CheckIn != default && x.CheckOut != default
                        ? CalculateHoursWorked(x.CheckIn, x.CheckOut).ToString("F2", CultureInfo.InvariantCulture)
                        : "0.00",
                    WeeklyJourney = x.WeeklyJourney,

                })
                .ToList();

            var result = new ExportPunchClockAll
            {
                Employees = punchClockSummaries,
            };
            return result;

        }


    }
}
