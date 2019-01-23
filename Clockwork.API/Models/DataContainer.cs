using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clockwork.API.Models
{
    public class DataContainer
    {
        /// <summary>
        /// Gets or sets Result either FAIL or SUCCESS 
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets Message of the result
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the DB data based on query
        /// </summary>
        public List<CurrentTimeQuery> Data { get; set; }

        public List<QueryTimeZone> QueryTimeZones { get; set; }
    }
}

