using Food_Scape.Models;
using Food_Scape.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Food_Scape.Repositories
{
    public class RoleRepository
    {
        FoodScapeContext _context;

        public RoleRepository(FoodScapeContext context)
        {
            this._context = context;
            var rolesCreated = CreateInitialRoles();
        }

        public List<RoleVM> GetAllRoles()
        {
            var roles = _context.Roles;
            List<RoleVM> roleList = new List<RoleVM>();

            foreach (var item in roles)
            {
                roleList.Add(new RoleVM()
                {
                    RoleName = item.Name,
                    Id = item.Id
                });
            }
            return roleList;
        }

        public RoleVM GetRole(string roleName)
        {
            var role = _context.Roles.Where(r => r.Name == roleName)
                                     .FirstOrDefault();
            if (role != null)
            {
                return new RoleVM()
                {
                    RoleName = role.Name,
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
            _context.Roles.Add(new IdentityRole
            {
                Name = roleName,
                Id = roleName,
                NormalizedName = roleName.ToUpper()
            });
            _context.SaveChanges();
            return true;
        }

        public bool DeleteRole(string roleName)
        {
            try
            {
                var role = _context.Roles.Where(x => x.Id == roleName).FirstOrDefault();
                _context.Roles.Remove(role);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

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
