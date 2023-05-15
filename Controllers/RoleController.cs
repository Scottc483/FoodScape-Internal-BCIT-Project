using Microsoft.AspNetCore.Mvc;
using Food_Scape.Repositories;
using Food_Scape.Models;
using EllipticCurve;
using Food_Scape.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Food_Scape.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        FoodScapeContext _context;

        public RoleController(FoodScapeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Role Admin Pages / CRUD
        /// 
        /// Grabs all roles from AspNetRoles using
        /// Role Repository
        /// </summary>
        /// <returns>ActionResult</returns>
        [Route("admin/roles/list")]
        public ActionResult Index(string msg, string err)
        {

            // switch value of msg and err into empty string if null
            if (msg == null)
            {
                msg = "";
            }

            if (err == null)
            {
                err = "";
            }

            // store err and msg in ViewBag to be displayed
            ViewBag.Message = msg;
            ViewBag.Error = err;

            RoleRepository roleRepo = new RoleRepository(_context);
            return View(roleRepo.GetAllRoles());
        }

        // CREATE: Role
        [HttpGet]
        [Route("admin/roles/create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/roles/create")]
        public ActionResult Create(RoleVM newRole)
        {
            //var token = HttpContext.Request.Form["__RequestVerificationToken"];
            RoleRepository roleRepo = new RoleRepository(_context);
            var results = roleRepo.CreateRole(newRole.RoleName);

            if (!results)
            {
                return RedirectToAction("Index", "Role", new { err = "Failed creating role."});
            }
            else
            {
                return RedirectToAction("Index", "Role", new { msg = "Successfully created role!" });
            }
        }

        [Route("admin/roles/delete")]
        public IActionResult Delete(string id)
        {
            RoleRepository roleRepo = new RoleRepository(_context);
            RoleVM role = roleRepo.GetRole(id);
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/roles/delete")]
        public IActionResult Delete(RoleVM role)
        {
            RoleRepository roleRepo = new RoleRepository(_context);
            var results = roleRepo.DeleteRole(role.RoleName);

            if (!results)
            {
                return RedirectToAction("Index", "Role", new { err = "Failed deleting role." });
            }
            else
            {
                return RedirectToAction("Index", "Role", new { msg = "Successfully deleted role!" });
            }

        }

    }
}
