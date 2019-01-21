using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clockwork.Web.Models
{
    public class CurrentTimeQuery
    {
        public int CurrentTimeQueryId { get; set; }
        public string Time { get; set; }
        public string ClientIp { get; set; }
        public string UTCTime { get; set; }
        public string Timezone { get; set; }

    }
}