using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Models
{
    public class UserFormatInfo
   {
      // account format for UserSource.Format user type
      public string UserAccountFormat { get; set; } = "abc_{0:D7}@somecompany.co";

      // total users for ....Format 
      public long TotalUsers { get; set; } = 1000;

      // users start index for ....Format 
      public long UserStartIndex { get; set; } = 6000;
   }
}