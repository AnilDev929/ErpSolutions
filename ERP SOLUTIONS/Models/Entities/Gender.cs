using System.ComponentModel.DataAnnotations;

namespace ERP_SOLUTIONS.Models.Entities
{
    public class Gender
    {
        [Key]
        public int GenderID { get; set; }

        [Required]
        public int GenderName { get; set; }
    }
}
