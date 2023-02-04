using Day3Paypal.Data;
using Day3Paypal.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Day3Paypal.Repositories
{
    public class RoleRepo
    {
        ApplicationDbContext _db;

        public RoleRepo(ApplicationDbContext db)
        {
            this._db = db;
            var rolesCreated = CreateInitialRoles();
        }

        public List<RoleVM> GetAllRoles()
        {
            var roles = _db.Roles;
            List<RoleVM> roleList = new List<RoleVM>();

            foreach (var item in roles)
            {
                roleList.Add(new RoleVM()
                {
                    RoleName = item.Name
                                          ,
                    Id = item.Id
                });
            }
            return roleList;
        }
        public RoleVM GetRole(string roleName)
        {
            var role = _db.Roles.Where(r => r.Name == roleName)
                                     .FirstOrDefault();
            if (role != null)
            {
                return new RoleVM()
                {
                    RoleName = role.Name
                                    ,
                    Id = role.Id
                };
            }
            return null;
        }

        public bool CreateRole(string roleName)
        {
            var role = GetRole(roleName);
            if (role != null)
            {
                return false;
            }
            _db.Roles.Add(new IdentityRole
            {
                Name = roleName,
                Id = roleName,
                NormalizedName = roleName.ToUpper()
            });
            _db.SaveChanges();
            return true;
        }

        public bool CreateInitialRoles()
        {
            // Create roles if none exist.
            // This is a simple way to do it but it 
            // would be better to use a seeder.
            string[] roleNames = { "Admin", "Manager", "Customer" };
            foreach (var roleName in roleNames)
            {
                var created = CreateRole(roleName);
                // Role already exists so exit.
                if (!created)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
