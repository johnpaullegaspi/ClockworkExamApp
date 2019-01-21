using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clockwork.API.Models
{
    public class QueryTimeZone
    {
        public string Id { get; set; }
        public string StandardName { get; set; }
        public string DisplayName { get; set; }
    }
}
