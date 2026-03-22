using ERP_SOLUTIONS.Data;
using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_SOLUTIONS.Controllers
{
    [Authorize]
    public class AcademicYearsController : Controller
    {
        private readonly AppDbContext _context;

        public AcademicYearsController(AppDbContext context)
        {
            _context = context;
        }

        // LIST + FILTER
        public async Task<IActionResult> Index(string search, bool? status)
        {
            var data = _context.AcademicYears.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(x => x.YearName.Contains(search));
            }

            if (status.HasValue)
            {
                data = data.Where(x => x.IsActive == status.Value);
            }

            return View(await data.OrderByDescending(x => x.YearStart).ToListAsync());
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // HELPER: Check for duplicate
        private async Task<bool> IsDuplicateAsync(string yearName, int id = 0)
        {
            return await _context.AcademicYears
                .AnyAsync(x => x.YearName.ToLower() == yearName.ToLower() && x.AcademicYearID != id);
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AcademicYear model)
        {
            // 1. Validate date range
            if (model.YearEnd <= model.YearStart)
            {
                ModelState.AddModelError("", "End date must be greater than Start date.");
            }

            // 2. Check for duplicate YearName
            bool isDuplicate = await _context.AcademicYears
                .AnyAsync(x => x.YearName.ToLower() == model.YearName.ToLower());
            if (isDuplicate)
            {
                ModelState.AddModelError("YearName", $"Academic Year '{model.YearName}' already exists.");
            }

            if (ModelState.IsValid)
            {
                // 3. Ensure only one active year
                if (model.IsActive)
                {
                    var activeYears = await _context.AcademicYears
                        .Where(x => x.IsActive)
                        .ToListAsync();

                    foreach (var item in activeYears)
                    {
                        item.IsActive = false;
                    }
                }

                // 4. Save
                _context.AcademicYears.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            // 5. Return to form if validation failed
            return View(model);
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _context.AcademicYears.FindAsync(id);
            if (data == null) return NotFound();

            return View(data);
        }


        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AcademicYear model)
        {
            // Validate date range
            if (model.YearEnd <= model.YearStart)
            {
                ModelState.AddModelError("", "End date must be greater than Start date.");
            }

            // Check duplicate excluding current record
            if (await IsDuplicateAsync(model.YearName, model.AcademicYearID))
            {
                ModelState.AddModelError("YearName", $"Academic Year '{model.YearName}' already exists.");
            }

            if (ModelState.IsValid)
            {
                // Ensure only one active year
                if (model.IsActive)
                {
                    var activeYears = await _context.AcademicYears
                        .Where(x => x.IsActive && x.AcademicYearID != model.AcademicYearID)
                        .ToListAsync();

                    foreach (var item in activeYears)
                    {
                        item.IsActive = false;
                    }
                }

                _context.AcademicYears.Update(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }


        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.AcademicYears.FindAsync(id);
            if (data != null)
            {
                _context.AcademicYears.Remove(data);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
