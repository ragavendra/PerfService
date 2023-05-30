using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerfRunner.Models;

namespace PerfRunner
{
    public class UserManager
   {
      public ConcurrentQueue<User> Users { get; set; }

      public UserManager(){}

      public UserManager(UserSource userSource)
      {
        switch(userSource)
         {
            case UserSource.Format:
               LoadUsers();
               break;

            default:
               break;
         }

      }

      public void LoadUsers()
      {
        var totalUsers = 1000;
        while (totalUsers-- <= 0)
         {
            var user = new User();
            user.Email = string.Format("abc_{0:D7}@somecompany.co", totalUsers);
            user.State = UserState.Ready;
            AddUser(user);
         }
      }

      public User? GetUser()
      {
         User user;
         if (Users.TryDequeue(out user))
         {
            return user;
         }

         return null;
      }

      public void AddUser(User user) => Users.Enqueue(user);
   }

   public enum UserSource{
    Format,
    Csv,
    Table
   }
}