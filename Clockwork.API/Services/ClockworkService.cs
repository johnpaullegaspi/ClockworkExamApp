using Clockwork.API.Interfaces;
using Clockwork.API.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Clockwork.API.Services
{
    class ClockworkService : IClockworkService
    {
        /// <summary>
        /// Save Current Time Query data to database
        /// </summary>
        /// <param name="utcTime"></param>
        /// <param name="ip"></param>
        /// <param name="serverTime"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public CurrentTimeQuery AddCurrentTimeQuery(DateTime utcTime, string ip, DateTime serverTime, string timeZoneId)
        {
            var currentTimeQuery = new CurrentTimeQuery
            {
                UTCTime = utcTime,
                ClientIp = ip,
                Time = serverTime,
                Timezone = !string.IsNullOrEmpty(timeZoneId) && timeZoneId.ToUpper().Contains("SELECT TIMEZONE") ? string.Empty : timeZoneId
            };

            using (var db = new ClockworkContext())
            {
                db.CurrentTimeQueries.Add(currentTimeQuery);
                db.SaveChanges();
            }

            return currentTimeQuery;

        }

        /// <summary>
        /// Retrieve all CurrentTimeQueries data from database
        /// </summary>
        /// <returns>CurrentTimeQueries data</returns>
        public List<CurrentTimeQuery> GetCurrentTimeQueries()
        {
            var currentTimeQueries = new List<CurrentTimeQuery>();
            var clockworkService = new ClockworkService();

            using (var db = new ClockworkContext())
            {
                currentTimeQueries = db.CurrentTimeQueries.ToList<CurrentTimeQuery>();

                currentTimeQueries.ForEach(i => i.Time = clockworkService.GetDateTimeByTimeZone(i.Time, i.Timezone));
            }

            return currentTimeQueries;
        }

        /// <summary>
        /// Get Time zone list
        /// </summary>
        /// <returns>a list of Timezones</returns>
        public List<QueryTimeZone> GetTimeZoneList()
        {
            var timeZones = TimeZoneInfo.GetSystemTimeZones();

            var timeZoneList = timeZones.Select(
                i => new QueryTimeZone()
                {
                    Id = i.Id,
                    DisplayName = i.DisplayName,
                    StandardName = i.StandardName
                }).ToList<QueryTimeZone>();

            return timeZoneList;
        }

        /// <summary>
        /// Get Converted Date and Time based on Timezone
        /// </summary>
        /// <param name="serverTime"></param>
        /// <param name="timeZoneDisplay"></param>
        /// <returns>Converted Date Time</returns>
        public DateTime GetDateTimeByTimeZone(DateTime serverTime, string timeZoneDisplay)
        {
            var timezoneDateAndTime = serverTime;

            if (string.IsNullOrEmpty(timeZoneDisplay) || timeZoneDisplay.ToUpper().Contains("SELECT TIMEZONE"))
            {
                return timezoneDateAndTime;
            }

            var timeZoneInfo = TimeZoneInfo.GetSystemTimeZones().Where(i => i.DisplayName == timeZoneDisplay).FirstOrDefault();

            if (timeZoneInfo != null)
            {
                timezoneDateAndTime = TimeZoneInfo.ConvertTime(serverTime, timeZoneInfo);
            }

            return timezoneDateAndTime;
        }

        /// <summary>
        /// Get TimeZoneInfo object by time zone display
        /// </summary>
        /// <param name="timeZoneDisplay"></param>
        /// <returns>TimeZoneInfo object</returns>
        public TimeZoneInfo GetTimeZoneInfoById(string timeZoneDisplay)
        {
            var timeZoneInfo = TimeZoneInfo.GetSystemTimeZones().Where(i => i.DisplayName == timeZoneDisplay).FirstOrDefault();

            return timeZoneInfo;
        }

        public string CheckMessage(string message)
        {
            var errorMessage = string.Empty;

            if (message.ToLower().Contains("unable to connect"))
            {
                errorMessage = "Opss! Something's wrong with the API!";
            }
            else
            {
                errorMessage = "Error encountered: " + message;
            }

            return errorMessage;
        }
    }


}
