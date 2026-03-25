using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Models.ViewModels;

namespace ERP_SOLUTIONS.Services.Interfaces
{
    public interface IHostelService
    {
        Task<IEnumerable<HostelWithRoomsVM>> GetAllHostelsAsync();
        Task CreateHostelAsync(HostelWithRoomsVM model);
        Task<HostelWithRoomsVM> GetHostelWithRoomsAsync(int hostelId);
        Task AddHostelAsync(Hostel hostel, List<RoomVM> rooms);
        Task UpdateHostelAsync(HostelWithRoomsVM model);
        Task DeleteRoomAsync(int roomId);
    }
}
