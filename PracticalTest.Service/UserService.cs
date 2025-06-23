using PracticalTest.External.Contracts;
using PracticalTest.Service.Contracts;
using PracticalTest.Domain.Models;
using Microsoft.Extensions.Logging;

namespace PracticalTest.Service
{
   public class UserService : IUserService
   {
      private readonly ILogger<UserService> _logger;
      private readonly IWrapperReqResService _wrapperReqResService;

      public UserService(ILogger<UserService> logger,
         IWrapperReqResService wrapperReqResService)
      {
         _logger = logger;
         _wrapperReqResService = wrapperReqResService;
      }

      /// <summary>
      /// Get details of user by id
      /// </summary>
      /// <param name="userId"></param>
      /// <returns></returns>
      public async Task<User> GetUserAsync(int userId)
      {
         try
         {
            return await _wrapperReqResService.GetUserAsync(userId);
         }
         catch (Exception ex)
         {
            _logger.LogError($"Error :- {ex.Message}");
            throw;
         }
      }

      /// <summary>
      /// Get list of users
      /// </summary>
      /// <returns></returns>
      public async Task<List<Data>> GetUsersAsync()
      {
         try
         {
            int pageNumber = 1;
            int totalPages;
            List<Data> users = new List<Data>();

            do
            {
               var result = await _wrapperReqResService.GetUsersAsync(pageNumber);
               totalPages = result.TotalPages;

               users.AddRange(result.Data);

               pageNumber++;

            } while (pageNumber <= totalPages);

            return users;

         }
         catch (Exception ex)
         {
            _logger.LogError($"Error :- {ex.Message}");
            throw;
         }

      }

   }
}
