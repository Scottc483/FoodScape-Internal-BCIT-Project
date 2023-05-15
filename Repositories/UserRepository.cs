using EllipticCurve;
using Food_Scape.Models;
using Food_Scape.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Food_Scape.Repositories
{
    public class UserRepository
    {
        FoodScapeContext _context;
        IServiceProvider _serviceProvider;

        public UserRepository(FoodScapeContext context, IServiceProvider? serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        // grabs all FsUsers from db and returns as List
        public List<FsUser> GetAllUsers()
        {
            var users = _context.FsUsers.ToList();
            return users;
        }

        // grab FsUser from db with UserId equal to parameter id
        public FsUser GetUserById(int? id)
        {
            
            var user = _context.FsUsers.Where(u => u.UserId == id).FirstOrDefault();

            return user;
        }

        // grab FsUser from db with Email equal to parameter email
        public FsUser GetUserByEmail(string email)
        {
            var user = _context.FsUsers.Where(u => u.Email == email).FirstOrDefault();
            return user;
        }

        // takes in FsUser as parameter with new values and updates
        // the database record with parameter
        public void EditUserInfo(FsUser fsUser)
        {
            _context.Update(fsUser);
            _context.SaveChanges();
        }

        // Deletes User from FsUser table and also from AspNetUsers table
        public async Task<bool> DeleteUser(FsUser fsUser)
        {
            if (fsUser.Email == null) return false;

            var userManager = _serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();


            var user = await userManager.FindByEmailAsync(fsUser.Email);

            if(user != null)
            {
                var res = await userManager.DeleteAsync(user);
                _context.Remove(fsUser);
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
