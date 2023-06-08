using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Models
{
    public class UserFormatInfo
   {
      private readonly string _userAccountFormat =  "abc_{0:D7}@somecompany.co";

      private readonly long _totalUsers = 1000;

      private readonly long _userStartIndex = 6000;

      // account format for UserSource.Format user type
      public string UserAccountFormat
      {
         get { return _userAccountFormat; }
      }

      // total users for ....Format 
      public long TotalUsers { get { return _totalUsers; } }

      // users start index for ....Format 
      public long UserStartIndex { get { return _userStartIndex; } }
   }
}