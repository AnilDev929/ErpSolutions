using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_SOLUTIONS.Models.Entities
{
    [Table("MenuSections")]
    public class MenuSection
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string Subtitle { get; set; }
        public bool IsActive { get; set; }

        //Avoid null reference issues
        public List<MenuItem> Items { get; set; } = new();
    }
}
