using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Clockwork.API.Models
{
    public class ClockworkContext : DbContext
    {
        public ClockworkContext(DbContextOptions<ClockworkContext> options)
        : base(options)
        {
        }

        public ClockworkContext()
        {

        }

        public DbSet<CurrentTimeQuery> CurrentTimeQueries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

                // 1) Use SQL Server Database: use below codes
                /*
                    var connectionString = configuration.GetConnectionString("ClockWorkDatabase");
                    optionsBuilder.UseSqlServer(connectionString);
                */

                // 2) Use SQLite Database: use below codes
                var connectionString = configuration.GetConnectionString("ClockWorkSQLiteDatabase");
                optionsBuilder.UseSqlite(connectionString);

            }

        }
    }

    
}
