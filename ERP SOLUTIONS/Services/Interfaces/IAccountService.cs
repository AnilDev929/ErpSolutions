namespace ERP_SOLUTIONS.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(bool Success, string Message)> LoginAsync(string username, string password, string roleName);
    }
}
