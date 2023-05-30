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
      private ConcurrentQueue<User> Users { get; set; } = new ConcurrentQueue<User>(){};

      public UserFormatInfo UserFormatInfo { get; set; } = new UserFormatInfo();

      public UserManager()
      {
         // let me load pre conf users for now
         if (Users?.Count <= 0)
         {
            LoadUsers();
         }
      }

      public void UserManager_(UserSource userSource)
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
        var totalUsers = UserFormatInfo?.TotalUsers;
        var accountIndex = UserFormatInfo?.UserStartIndex;
        while (totalUsers-- >= 0)
         {
            var user = new User();
            user.Email = string.Format(UserFormatInfo!.UserAccountFormat, accountIndex++);
            user.State = UserState.Ready;
            AddUser(user);
         }
      }

      public User? GetUser()
      {
         User user;
         if (Users?.Count > 0)
         {
            if (Users.TryDequeue(out user))
            {
               return user;
            }
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