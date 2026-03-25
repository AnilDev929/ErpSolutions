using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Models.ViewModels;
using ERP_SOLUTIONS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP_SOLUTIONS.Controllers
{
    public class HostelController : Controller
    {
        private readonly IHostelService _hostelService;
        public HostelController(IHostelService hostelService)
        {
            _hostelService = hostelService;
        }

        // GET: List Page
        public async Task<ActionResult> Index()
        {
            var data = await _hostelService.GetAllHostelsAsync();
            return View(data);
        }

        // GET: Create Page
        public ActionResult Create()
        {
            var model = new HostelWithRoomsVM
            {
                Hostel = new Hostel(),
                Rooms = new List<RoomVM>()
            };

            return View(model);
        }

        // POST: Save Hostel + Rooms
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HostelWithRoomsVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _hostelService.CreateHostelAsync(model);
                TempData["Success"] = "Hostel and Rooms created successfully!";
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return View(model);
            }
        }


        public async Task<ActionResult> Edit(int id)
        {
            var model = await _hostelService.GetHostelWithRoomsAsync(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HostelWithRoomsVM model)
        {
            try
            {
                await _hostelService.UpdateHostelAsync(model);
                TempData["Success"] = "Updated successfully!";
                return RedirectToAction("Edit", new { id = model.Hostel.HostelId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }



    }
}
