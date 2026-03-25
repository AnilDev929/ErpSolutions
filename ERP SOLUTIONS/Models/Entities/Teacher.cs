
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_SOLUTIONS.Models.Entities
{
    public class Teacher
    {
        [Key]
        public int TeacherID { get; set; }

        [Required]
        public int UserID { get; set; }

        [StringLength(100)]
        public string? Designation { get; set; }

        [Required]
        [StringLength(150)]
        public string FullName { get; set; }

        [Required]
        public int GenderID { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(3)]
        public string? BloodGroup { get; set; }

        [StringLength(150)]
        public string? FatherName { get; set; }

        [StringLength(150)]
        public string? MotherName { get; set; }

        [StringLength(10)]
        public string? EmergencyContact { get; set; }

        public bool IsMarried { get; set; } = false;

        [StringLength(100)]
        public string? SpouseName { get; set; }

        [StringLength(10)]
        public string? SpouseContact { get; set; }

        [StringLength(30)]
        public string? Aadhaar { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? EmailID { get; set; }

        [StringLength(250)]
        public string? PresentAddress { get; set; }

        [StringLength(250)]
        public string? PermanentAddress { get; set; }

        [StringLength(100)]
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }

        [Column(TypeName = "decimal(3,1)")]
        public decimal? Experience { get; set; }

        public DateTime? JoiningDate { get; set; }

        public DateTime? LastWorkingDate { get; set; }

        public bool IsActive { get; set; } = true;

        // 🔗 Navigation Properties (Optional but Recommended)

        public virtual User User { get; set; }
        public virtual Gender Gender { get; set; }
    }
}
