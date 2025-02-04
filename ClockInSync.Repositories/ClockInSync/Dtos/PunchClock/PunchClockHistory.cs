using ClockInSync.Repositories.Dtos.PunchClock;
using ClockInSync.Repositories.Dtos.User.UserResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockInSync.Repositories.ClockInSync.Dtos.PunchClock
{
    public class PunchClockHistory 
    {
        public List<Registers> Registers { get; set; } = [];
    }
}
