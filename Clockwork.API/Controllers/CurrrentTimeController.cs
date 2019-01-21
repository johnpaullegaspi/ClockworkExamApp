using System;
using Microsoft.AspNetCore.Mvc;
using Clockwork.API.Models;
using Clockwork.API.Services;

namespace Clockwork.API.Controllers
{
    [Route("api/[controller]")]
    public class CurrentTimeController : Controller
    {
        ClockworkService _clockworkService = null;

        // GET api/currenttime/GetCurrentTimeQueries
        [HttpGet]
        [Route("GetCurrentTimeQueries")]
        public IActionResult Get()
        {
            var utcTime = DateTime.UtcNow;
            var serverTime = DateTime.Now;
            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();

            _clockworkService = new ClockworkService();

            var currentTimeQueries = _clockworkService.GetCurrentTimeQueries();
            var timeZonesList = _clockworkService.GetTimeZoneList();

            var dataContainer = new DataContainer
            {
                Data = currentTimeQueries,
                QueryTimeZones = timeZonesList
            };

            return Ok(dataContainer);
        }

        // GET api/currenttime/AddTimeByTimeZone?timeZoneId=value
        [HttpGet]
        [Route("AddTimeByTimeZone")]
        public IActionResult Add(string timeZoneId)
        {
            var utcTime = DateTime.UtcNow;
            var serverTime = DateTime.Now;
            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
            _clockworkService = new ClockworkService();

            var currentTimeQuery = _clockworkService.AddCurrentTimeQuery(utcTime, ip, serverTime, timeZoneId);
            var currentTimeQueries = _clockworkService.GetCurrentTimeQueries();
            var timeZonesList = _clockworkService.GetTimeZoneList();

            var dataContainer = new DataContainer
            {
                Data = currentTimeQueries,
                QueryTimeZones = timeZonesList
            };

            return Ok(dataContainer);
        }

    }
}
