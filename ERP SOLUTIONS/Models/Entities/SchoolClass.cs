using System.ComponentModel.DataAnnotations;

namespace ERP_SOLUTIONS.Models.Entities
{
    public class SchoolClass
    {
        [Key]
        public int ClassID { get; set; }

        [Required]
        [Display(Name = "Class Name")]
        [StringLength(100, ErrorMessage = "Class name cannot exceed 30 characters")]
        public string ClassName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Stream cannot exceed 10 characters")]
        public string Stream { get; set; }

        [StringLength(10)]
        [Display(Name = "Code")]
        public string? DeptCode { get; set; }

        public int? Duration { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = false;
    }
}
