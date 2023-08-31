using PerfRunner.Models;

namespace PerfRunner.Services
{
    public interface IUserManager
    {
        public User CheckOutUser(UserState userState);

        public bool CheckInUser(User user);
    }
}