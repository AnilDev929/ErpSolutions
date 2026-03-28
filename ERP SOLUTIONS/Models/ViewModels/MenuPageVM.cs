namespace ERP_SOLUTIONS.Models.ViewModels
{
    public class MenuPageVM
    {
        public MenuSectionVM NewSection { get; set; } = new();  // For form
        public List<MenuSectionVM> ExistingSections { get; set; } = new();  // For table
    }
}
