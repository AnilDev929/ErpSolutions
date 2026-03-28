using ERP_SOLUTIONS.Data;
using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERP_SOLUTIONS.Services.Implementations
{
    public class AccountService : IAccountService
    {

        private readonly AppDbContext _context;
        private readonly ILogger<SubjectService> _logger;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountService(AppDbContext context, ILogger<SubjectService> logger)
        {
            _context = context;
            _logger = logger;

            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<(bool Success, string Message)> LoginAsync(string username, string password, string roleName)
        {
            // 1. Get user from DB
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return (false, "Invalid username or password");
            }

            //// 2. Verify password
            //var hasher = new PasswordHasher<User>();
            //var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            //if (result != PasswordVerificationResult.Success)
            //{
            //    return (false, "Invalid username or password");
            //}

            // 2️⃣ Check if account is active / locked
            if (!user.IsActive)
                return (false, "Account is inactive");

            if (user.LockoutUntil != null && user.LockoutUntil > DateTime.Now)
                return (false, $"Account is locked until {user.LockoutUntil}");

            // 3️⃣ Verify password
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result != PasswordVerificationResult.Success)
            {
                // Increment failed login count
                user.FailedLoginCount++;
                await _context.SaveChangesAsync();
                return (false, "Invalid username or password");
            }

            // 4️⃣ Check Role
            bool hasRole = user.UserRoles.Any(ur => ur.Role.RoleName == roleName);
            if (!hasRole)
                return (false, "User does not have this role");

            // 5️⃣ Reset failed login count & update last login
            user.FailedLoginCount = 0;
            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            return (true, "Login successful");
        }


        //public async Task<(bool Success, string Message)> ValidateLogin(string username, string password, string roleName)
        //{
        //    // 1️⃣ Fetch user with roles
        //    var user = await _context.Users
        //        .Include(u => u.UserRol)
        //            .ThenInclude(ur => ur.Role)
        //        .FirstOrDefaultAsync(u => u.Username == username);

        //    if (user == null)
        //        return (false, "User not found");

        //    if (!user.IsActive)
        //        return (false, "Account is inactive");

        //    if (user.LockoutUntil != null && user.LockoutUntil > DateTime.Now)
        //        return (false, $"Account is locked until {user.LockoutUntil}");

        //    // 2️⃣ Verify password
        //    var hasher = new PasswordHasher<object>();
        //    var result = hasher.VerifyHashedPassword(null, user.PasswordHash, password);

        //    if (result != PasswordVerificationResult.Success)
        //    {
        //        // Increment failed login count
        //        user.FailedLoginCount++;
        //        await _context.SaveChangesAsync();
        //        return (false, "Incorrect password");
        //    }

        //    // 3️⃣ Check role
        //    bool hasRole = user.UserRoles.Any(ur => ur.Role.Name == roleName);
        //    if (!hasRole)
        //        return (false, "User does not have the required role");

        //    // 4️⃣ Reset failed login count & update last login
        //    user.FailedLoginCount = 0;
        //    user.LastLogin = DateTime.Now;
        //    await _context.SaveChangesAsync();

        //    return (true, "Login successful");
        //}



    }
}
