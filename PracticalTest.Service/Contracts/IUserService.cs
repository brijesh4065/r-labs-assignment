using PracticalTest.Domain.Models;

namespace PracticalTest.Service.Contracts
{
   public interface IUserService
   {
      Task<User> GetUserAsync(int userId);

      Task<List<Data>> GetUsersAsync();
   }
}
