using Clockwork.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clockwork.API.Interfaces
{
    public interface IClockworkService
    {
        List<CurrentTimeQuery> GetCurrentTimeQueries();
        CurrentTimeQuery AddCurrentTimeQuery(DateTime utcTime, string ip, DateTime serverTime, string timeZoneId);
    }
}
