using PracticalTest.Domain.Models;

namespace PracticalTest.External.Contracts
{
    public interface IWrapperReqResService
    {
        Task<UserList> GetUsersAsync(int pageNumber);

        Task<User> GetUserAsync(int userId);
    }
}
