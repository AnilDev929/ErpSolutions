
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_SOLUTIONS.Models.Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string UserName { get; set; }

        [NotMapped]
        public string Role { get; set; } = "Admin";

        public bool IsActive { get; set; } = true;

        //public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; } = null;

        public int FailedLoginCount { set; get; } = 0;

        public DateTime? LockoutUntil { get; set; } = null;

        [NotMapped]
        public Role role { get; set; } = new();

        // Navigation property
        public ICollection<UserRole> UserRoles { get; set; }

        public User() { }
    }
}
