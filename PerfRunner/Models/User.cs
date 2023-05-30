using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Models
{
   // A class containing the user context data, it can be address, contact info
   // related to user based on the web app api. Lets, say the web app to be a bowling
   // ally.
   public class User : IUser
   {
      public Guid Guid { get; set; }

      public string Email { get; set; }

      public UserState State { get; set; }

      public User(){
         Guid = Guid.NewGuid();
         State = UserState.Ready;
      }
   }
}