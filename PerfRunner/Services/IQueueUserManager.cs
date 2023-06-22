using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerfRunner.Models;

namespace PerfRunner.Services
{
    public interface IQueueUserManager
    {

      public User CheckOutUser(UserState userState);

      // public bool CheckInUser(User user);
    }
}