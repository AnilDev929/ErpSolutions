namespace ERP_SOLUTIONS.Models.ViewModels
{
    public class MenuSectionVM
    {// Section fields
        public int Id { set; get; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string Subtitle { get; set; }
        public bool IsActive { get; set; }

        // Items list
        public List<MenuItemVM> Items { get; set; } = new();

        // Existing items to display
        public List<MenuItemVM> ExistingItems { get; set; } = new();
    }
}
