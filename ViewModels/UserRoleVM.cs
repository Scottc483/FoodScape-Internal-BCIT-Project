using System.ComponentModel.DataAnnotations;
using Food_Scape.Models;
using Food_Scape.ViewModels;

namespace Food_Scape.ViewModels
{
    public class UserRoleVM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string RoleName { get; set; }

        public List<RoleVM>? Roles { get; set; }
        public List<FsUser>? Users { get; set; }
    }
}
