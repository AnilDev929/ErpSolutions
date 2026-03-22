using ERP_SOLUTIONS.Data;
using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP_SOLUTIONS.Controllers
{
    [Authorize]
    public class ClassesController : Controller
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        // ✅ READ (LIST)
        public async Task<IActionResult> Index()
        {
            var classes = await _classService.GetAllAsync();
            return View(classes);
        }

        // ✅ READ (DETAILS)
        public async Task<IActionResult> Details(int id)
        {
            var cls = await _classService.GetByIdAsync(id);

            if (cls == null)
                return NotFound();

            return View(cls);
        }

        // ✅ CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // ✅ CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SchoolClass model)
        {
            if (ModelState.IsValid)
            {
                var result = await _classService.AddAsync(model);

                if (!result)
                {
                    ModelState.AddModelError("", "Class already exists!");
                    return View(model);
                }

                TempData["Success"] = "Class created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // ✅ EDIT (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var cls = await _classService.GetByIdAsync(id);

            if (cls == null)
                return NotFound();

            return View(cls);
        }

        // ✅ EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SchoolClass model)
        {
            if (id != model.ClassID)
                return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _classService.UpdateAsync(model);

                if (!result)
                {
                    ModelState.AddModelError("", "Duplicate class exists!");
                    return View(model);
                }

                TempData["Success"] = "Class updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // ✅ DELETE (GET - Confirmation Page)
        public async Task<IActionResult> Delete(int id)
        {
            var cls = await _classService.GetByIdAsync(id);

            if (cls == null)
                return NotFound();

            return View(cls);
        }

        // ✅ DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _classService.DeleteAsync(id);

            TempData["Success"] = "Class deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ✅ EXISTS CHECK (OPTIONAL API)
        [HttpGet]
        public async Task<IActionResult> Exists(int id)
        {
            var exists = await _classService.ExistsAsync(id);
            return Json(exists);
        }
    }
}
