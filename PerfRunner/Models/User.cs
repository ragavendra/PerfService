using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace PerfRunner.Models
{
   // A class containing the user context data, it can be address, contact info
   // related to user based on the web app api. Lets, say the web app to be a bowling
   // ally.
   public class User : IUser
   {
      private readonly string _email;

      private UserState _state;

      public Guid Guid { get; } = Guid.NewGuid(); 

      // public string Email { get { return _email; } set { if(value.Length > 3 ) { _email = value; } } }
      public string Email { get { return _email; } }

      public UserState State { get { return _state; } set { _state = value; }}

      public User(string email, UserState state){
         _email = email;
         _state = state;
      }

      public static explicit operator User(RedisValue v)
      {
         throw new NotImplementedException();
      }
   }
}