using Food_Scape.Models;
using Food_Scape.Repositories;
using Food_Scape.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Globalization;

namespace Food_Scape.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        FoodScapeContext _foodScapeContext;

        public AdminController(FoodScapeContext foodScapeContext)
        {
            _foodScapeContext= foodScapeContext;
        }

        [Route("admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
