using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public class UserFormatInfo
   {
      private static readonly string _userAccountFormat =  "abc_{0:D7}@somecompany.co";

      private static readonly long _totalUsers = 1000;

      private static readonly long _userStartIndex = 6000;

      // account format for UserSource.Format user type
      public static string UserAccountFormat
      {
         get { return _userAccountFormat; }
      }

      // total users for ....Format 
      public static long TotalUsers { get { return _totalUsers; } }

      // users start index for ....Format 
      public static long UserStartIndex { get { return _userStartIndex; } }
   }
}