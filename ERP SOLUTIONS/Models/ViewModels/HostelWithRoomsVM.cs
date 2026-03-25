using ERP_SOLUTIONS.Models.Entities;

namespace ERP_SOLUTIONS.Models.ViewModels
{
    public class HostelWithRoomsVM
    {
        public Hostel Hostel { get; set; }
        public List<RoomVM> Rooms { get; set; }
    }
}
