using ERP_SOLUTIONS.Data;
using ERP_SOLUTIONS.Helpers;
using ERP_SOLUTIONS.Models.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace ERP_SOLUTIONS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthController(AppDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO login)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.UserName == login.Username && x.IsActive);

            if (user == null)
                return Unauthorized("Invalid username or password");

            // TEMP plain text check (next step = hashing)
            if (user.PasswordHash != login.Password)
                return Unauthorized("Invalid username or password");

            var token = _jwtHelper.GenerateToken(user.UserName, user.Role);

            return Ok(new
            {
                token,
                role = user.Role
            });
        }
    }
}
