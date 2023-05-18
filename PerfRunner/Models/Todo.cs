using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Models
{
   public class Todo
   {
      public int userId { get; set; }
      public int id { get; set; }
      public string title { get; set; }
      public bool completed { get; set; }
   }
}