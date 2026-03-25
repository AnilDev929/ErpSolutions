namespace ERP_SOLUTIONS.Models.Entities
{
    public class DashboardItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }

    public class DashboardSection
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string Subtitle { get; set; }
        public List<DashboardItem> Items { get; set; }
    }
}
