using System.ComponentModel.DataAnnotations;

namespace ERP_SOLUTIONS.Models.Entities
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Subject Name is required")]
        [StringLength(150)]
        public string SubjectName { get; set; }

        [StringLength(10)]
        public string SubjectCode { get; set; }

        [StringLength(150)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
