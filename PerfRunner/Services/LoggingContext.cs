using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PerfRunner.Services
{

   public class LoggingContext : DbContext
   {
      public DbSet<Log> Logs { get; set; }
      public DbSet<Entry> Entries { get; set; }

      public string DbPath { get; }

      public LoggingContext()
      {
         var folder = Environment.SpecialFolder.LocalApplicationData;
         var path = Environment.GetFolderPath(folder);
         DbPath = Path.Join(path, "logging.db");
      }

      // The following configures EF to create a Sqlite database file in the
      // special "local" folder for your platform.
      protected override void OnConfiguring(DbContextOptionsBuilder options)
          => options.UseSqlite($"Data Source={DbPath}");
   }

   public class Log
   {
      public int LogId { get; set; }
      public string Url { get; set; }

      public List<Entry> Entries { get; } = new();
   }

   public class Entry
   {
      public int Id { get; set; }
      public string Title { get; set; }
      public string Content { get; set; }

      public int LogId { get; set; }
      public Log Log { get; set; }
   }
}