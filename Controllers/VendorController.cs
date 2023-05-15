
using Food_Scape.Models;
using Food_Scape.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Food_Scape.Controllers
{
    public class VendorController : Controller
    {
        FoodScapeContext _foodScapeContext;

        public VendorController(FoodScapeContext foodScapeContext)
        {
            _foodScapeContext = foodScapeContext;
        }

        //Method to send the vendors information to the view.
        [Route("/vendors")]
        public IActionResult Index(string searchString)
        {
            VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);
            var vendors = vendorRepo.GetRecordsVM();

            if (!string.IsNullOrEmpty(searchString))
            {
                vendors = vendorRepo.GetRecordsByTypeVM(searchString);
            }

            return View(vendors);
        }

        //Method to get the list of vendors in the admin panel.
        [Route("admin/vendor/list")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminIndex(string msg, string err, string searchString)
        {
            VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);
            var vendors = vendorRepo.GetRecordsList();

            if(!string.IsNullOrEmpty(searchString))
            {
                vendors = _foodScapeContext.Vendors
                          .Where(x => 
                            x.VendorId.ToString().Contains(searchString) || 
                            x.Name.Contains(searchString) ||
                            x.Description.Contains(searchString) ||
                            x.VendorType.Contains(searchString) ||
                            x.Location.Contains(searchString)).ToList();
            }

            if (msg == null)
            {
                msg = "";
            }

            if (err == null)
            {
                err = "";
            }

            return View(vendors);
        }

        //Method to pass the detail of a vendor to the Details View.
        [Route("admin/vendor/details/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);
            Vendor vendor = vendorRepo.GetRecord(id);

            return View(vendor);
        }

        //Method to display the view to create a new vendor.
        [Route("admin/vendor/create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        //Method to create a new vendor.
        [Route("admin/vendor/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);
                    vendorRepo.CreateRecord(vendor);
                    return RedirectToAction("AdminIndex", new { msg = "Successfully created Vendor!" });
                }
                catch
                {
                    return RedirectToAction("AdminIndex", new { err = "Failed creating Vendor!" });
                }
            }

            return View(vendor);
        }

        //Method to display the view to edit an existing vendor.
        [Route("admin/vendor/edit")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);
            Vendor vendor = vendorRepo.GetRecord(id);

            return View(vendor);
        }

        //Method to edit an existing vendor.
        [Route("admin/vendor/edit")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);
                    vendorRepo.EditRecord(vendor);
                    return RedirectToAction("AdminIndex", new { msg = "Successfully edited Vendor!" });
                }
                catch 
                {
                    return RedirectToAction("AdminIndex", new { err = "Failed editing Vendor!" });
                }
            }

            return View(vendor);
        }

        //Vendor to display the view to delete a vendor.
        [Route("admin/vendor/delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);
            Vendor vendor = vendorRepo.GetRecord(id);

            return View(vendor);
        }

        //Method to delete a vendor.
        [Route("admin/vendor/delete")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    VendorRepository vendorRepo = new VendorRepository(_foodScapeContext);
                    vendorRepo.DeleteRecord(vendor);
                    return RedirectToAction("AdminIndex", new { msg = "Successfully deleted Vendor!" });
                }
                catch
                {
                    return RedirectToAction("AdminIndex", new { err = "Failed deleting Vendor!" });
                }
            }

            return View(vendor);
        }
    }
}
