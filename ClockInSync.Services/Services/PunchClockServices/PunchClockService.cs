﻿using AutoMapper;
using ClockInSync.Repositories.ClockInSync.Dtos.PunchClock;
using ClockInSync.Repositories.ClockInSync.Repositories;
using ClockInSync.Repositories.Dtos.PunchClock;
using ClockInSync.Repositories.Entities;
using System.Text;
using static ClockInSync.Repositories.Entities.PunchClock;

namespace ClockInSync.Services.PunchClockServices
{

    public interface IPunchClockService
    {
        Task<bool> RegisterPunchClock(Guid userId);
        Task<PunchClockHistory> GetPunchClockSummaries(Guid userId,int limit);
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

        public async Task<PunchClockHistory> GetPunchClockSummaries(Guid userId,int limit)
        {
            return await punchClockRepository.GetPunchClockSummaries(userId,limit);
        }

        public async Task<bool> RegisterPunchClock(Guid userId)
        {
            var lastPunchClockType = await punchClockRepository.GetPunchClockPrevious(userId);
            ClockInSync.Repositories.Dtos.PunchClock.RegisterPunchClock registerPunchClock = new();
            if (lastPunchClockType == null)
            {
                registerPunchClock.Type = PunchType.CheckIn;
            }
            else
            {
                switch (lastPunchClockType)
                {
                    case PunchType.CheckIn: 
                        registerPunchClock.Type = PunchType.CheckOut;
                        break;
                    case PunchType.CheckOut:
                        registerPunchClock.Type = PunchType.CheckIn;
                        break;
                }
            }
                
            var punchClock = mapper.Map<PunchClock>(registerPunchClock);

            punchClock.UserId = userId;
            punchClock.Id = Guid.NewGuid();
            punchClock.Timestamp = DateTime.Now;
            punchClock.Message = punchClock.Type == PunchClock.PunchType.CheckIn ? "Entrada registrada com sucesso" : "Saída registrada com sucesso";

            return await punchClockRepository.RegisterPunchClockAsync(punchClock);
        }
    }
}
