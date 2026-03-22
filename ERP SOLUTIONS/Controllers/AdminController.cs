using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP_SOLUTIONS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Role = "SuperAdmin")] // 🔒 IMPORTANT
    public class AdminController : ControllerBase
    {
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return Ok(new
            {
                message = "Welcome Admin",
                time = DateTime.Now
            });
        }

    }
}
