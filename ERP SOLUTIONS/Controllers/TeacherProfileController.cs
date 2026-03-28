using ERP_SOLUTIONS.Models.DTOS;
using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP_SOLUTIONS.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherProfileController : Controller
    {

        private readonly ITeacherService _teacherService;

        public TeacherProfileController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        public async Task<IActionResult> TeacherProfile()
        {
            int userId = int.Parse(User.FindFirst("UserID").Value);

            var teacher = await _teacherService.GetProfileByUserIdAsync(userId);
            return View(teacher);
        }

        public async Task<IActionResult> Edit()
        {
            int userId = int.Parse(User.FindFirst("UserID").Value);

            var teacher = await _teacherService.GetByUserIdAsync(userId);
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeacherProfileDTO model)
        {
            // Get current user's ID from claims
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _teacherService.UpdateProfileAsync(model, userId);

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Edit"); // stay on the same page
        }
    }
}
