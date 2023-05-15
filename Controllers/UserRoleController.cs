using Food_Scape.Models;
using Food_Scape.Repositories;
using Food_Scape.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Food_Scape.Controllers
{
    // This annotation can be used at the class or method level.
    // The annotation could include a comma separated list or different
    // roles.
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private FoodScapeContext _context;
        private IServiceProvider _serviceProvider;

        public UserRoleController(FoodScapeContext context,
                                    IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        // Show all roles for a specific user.
        [Route("admin/user/roles/{userName}")]
        public async Task<IActionResult> Detail(string userName)
        {
            UserRoleRepository userRoleRepo = new UserRoleRepository(_serviceProvider);
            var roles = await userRoleRepo.GetUserRoles(userName);
            ViewBag.UserName = userName;
            return View(roles);
        }

        // Present user with ability to assign roles to a user.
        // It gives two drop downs - the first contains the user names with
        // the requested user selected. The second drop down contains all
        // possible roles.
        [Route("admin/user/roles/create/")]
        public ActionResult Create(string userName)
        {
            // Store the email address of the Identity user
            // which is their user name.
            ViewBag.SelectedUser = userName;

            // Build SelectList with role data and store in ViewBag.
            RoleRepository roleRepo = new RoleRepository(_context);
            var roles = roleRepo.GetAllRoles().ToList();

            // There may be a better way but I have always found using the
            // .NET dropdown lists to be a challenge. Here is a way to make
            // it work if you can get the data in the proper format.

            // 1. Preparation for 'Roles' drop down.
            // a) Build a list of SelectListItem objects which have 'Value' and
            // 'Text' properties.
            var preRoleList = roles.Select(r =>
                new SelectListItem { Value = r.RoleName, Text = r.RoleName })
                   .ToList();
            // b) Store the SelectListItem objects in a SelectList object
            // with 'Value' and 'Text' properties set specifically.
            var roleList = new SelectList(preRoleList, "Value", "Text");

            // c) Store the SelectList in a ViewBag.
            ViewBag.RoleSelectList = roleList;

            // 2. Preparation for 'Users' drop down list.
            // a) Build a list of SelectListItem objects which have 'Value' and
            // 'Text' properties.
            var userList = _context.FsUsers.ToList();

            // b) Store the SelectListItem objects in a SelectList object
            // with 'Value' and 'Text' properties set specifically.
            var preUserList = userList.Select(u => new SelectListItem
            { Value = u.Email, Text = u.Email }).ToList();
            SelectList userSelectList = new SelectList(preUserList
                                                      , "Value"
                                                      , "Text");

            // c) Store the SelectList in a ViewBag.
            ViewBag.UserSelectList = userSelectList;
            return View();
        }

        // Assigns role to user.
        [Route("admin/user/roles/create/")]
        [HttpPost]
        public async Task<IActionResult> Create(UserRoleVM userRoleVM)
        {
            UserRoleRepository userRoleRepo = new UserRoleRepository(_serviceProvider);

            if (ModelState.IsValid)
            {
                var addUR = await userRoleRepo.AddUserRole(userRoleVM.Email,
                                                          userRoleVM.RoleName);
            }
            try
            {
                return RedirectToAction("Detail", "UserRole",
                       new { userName = userRoleVM.Email });
            }
            catch
            {
                return View();
            }
        }

        [Route("admin/user/roles/delete")]
        public ActionResult Delete(string userName, string roleName)
        {
            UserRoleVM userRole = new UserRoleVM
            {
                Email = userName,
                RoleName = roleName
            };
            return View(userRole);
        }

        [Route("admin/user/roles/delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(UserRoleVM userRoleVM)
        {
            UserRoleRepository userRoleRepo = new UserRoleRepository(_serviceProvider);

            if (ModelState.IsValid)
            {
                var deleteUR = await userRoleRepo.RemoveUserRole(userRoleVM.Email,
                                                          userRoleVM.RoleName);
            }
            try
            {
                return RedirectToAction("Detail", "UserRole",
                       new { userName = userRoleVM.Email });
            }
            catch
            {
                return View();
            }
        }
    }

}
