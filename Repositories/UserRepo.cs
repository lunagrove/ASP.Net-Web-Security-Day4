using Microsoft.EntityFrameworkCore;
using Day3Paypal.Data;
using Day3Paypal.ViewModels;

namespace Day3Paypal.Repositories
{
    public class UserRepo
    {
        ApplicationDbContext _db;

        public UserRepo(ApplicationDbContext context)
        {
            this._db = context;
        }

        public IEnumerable<UserVM> All()
        {
            IEnumerable<UserVM> users = _db.Users.Select(u => new UserVM() { Email = u.Email });
            return users;
        }

        public UserVM GetUser(string email)
        {
            var userVM = (from u in _db.MyRegisteredUsers
                         where u.Email == email
                         select new UserVM
                         {
                             Email = u.Email,
                             FirstName = u.FirstName,
                             LastName = u.LastName,
                         }).FirstOrDefault();

            return userVM;
        }
    }
}
