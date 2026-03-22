using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP_SOLUTIONS.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly IStudentProfileService _service;

        public StudentsController(IStudentProfileService service)
        {
            _service = service;
        }


        // GET: /Students/MyProfile
        //[Authorize] // ensure only logged-in users can access
        public async Task<IActionResult> MyProfile()
        {
            try
            {
                // Get the current logged-in user (based on username/email)
                var userName = User.Identity.Name;
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                {
                    // user is not logged in or claim missing
                    return Unauthorized();
                }
                
                // convert to int if needed
                int userId = int.Parse(userIdClaim);
                var userProfile = await _service.GetStudentProfileAsync(userId);

                if (userProfile == null)
                    return View("Error"); // or NotFound page

                return View(userProfile); // pass model to view
            }
            catch (Exception ex)
            {
                // Log exception
                //_logger.LogError(ex, "Error loading profile for user {UserName}", User.Identity.Name);
                return View("Error");
            }
        }

        // GET: /Account/EditProfile
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var userName = User.Identity.Name;
            var userProfile = await _service.GetStudentProfileAsync(1);

            if (userProfile == null)
                return View("Error");

            return View(userProfile);
        }

        // POST: /Account/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditProfile(Student model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _service.UpdateStudentProfileAsync(model);
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("MyProfile");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error updating profile for user {UserName}", User.Identity.Name);
                ModelState.AddModelError("", "Failed to update profile. Please try again later.");
                return View(model);
            }
        }

        public IActionResult Entry()
        {
            return View();
        }





    }
}
