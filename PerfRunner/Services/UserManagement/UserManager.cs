using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerfRunner.Models;

namespace PerfRunner.Services
{
   ///<summary>
   /// User mgmt for Users, keep this as light as possible
   /// One User instance per user, so millions can have millions
   /// need to check the performance as well.
   ///</summary>
    public class UserManager : IUserManager
   {
      private readonly ILogger<UserManager> _logger;

      private ConcurrentQueue<User> Users_ { get; set; } = new ConcurrentQueue<User>(){};

      private ConcurrentDictionary<UserState, ConcurrentQueue<User>> Users
      = new ConcurrentDictionary<UserState, ConcurrentQueue<User>>();

      private readonly IConfiguration _configuration;

      public UserFormatInfo UserFormatInfo { get; set; } = new UserFormatInfo();

      public UserManager(ILogger<UserManager> logger)
      {
         _logger = logger;

         Users.TryAdd(UserState.Ready, new ConcurrentQueue<User>());
         Users.TryAdd(UserState.Authenticated, new ConcurrentQueue<User>());
         Users.TryAdd(UserState.Playing, new ConcurrentQueue<User>());
         Users.TryAdd(UserState.Waiting, new ConcurrentQueue<User>());

         // let me load pre conf users for now
         // if (Users?.Count <= 0)
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

      // load users to ready state
      private void LoadUsers()
      {
         long totalUsers, accountIndex;
         if (!long.TryParse(_configuration.GetValue<string>("TotalUsers"), out totalUsers))
            totalUsers = UserFormatInfo.TotalUsers;

         if (!long.TryParse(_configuration.GetValue<string>("AccountStartIndex"), out accountIndex))
            accountIndex = UserFormatInfo.UserStartIndex;

         while (totalUsers-- >= 0)
         {
            var user = new User(string.Format(UserFormatInfo!.UserAccountFormat, accountIndex++), UserState.Ready);
            CheckInUser(user);
         }
      }

/*
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
      }*/

      public User CheckOutUser(UserState userState)
      {
         User user;
         var queue = GetUserQueue(userState);
         if (queue?.Count > 0)
         {
            if (queue.TryDequeue(out user))
            {
               return user;
            }
            else
            {
               _logger.LogDebug($"No more users in {userState} .");
            }
         }

         return null;
      }

      public bool CheckInUser(User user)
      {
         var queue = GetUserQueue(user.State);
         queue.Enqueue(user);
         return true;
      }

      // public void AddUser(User user) => Users.Enqueue(user);

      private ConcurrentQueue<User> GetUserQueue(UserState userState)
      {
         Users.TryGetValue(userState, out var queue);
         return queue;
      }
   }

   public enum UserSource{
    Format,
    Csv,
    Table
   }
}
