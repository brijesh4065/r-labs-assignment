
using PracticalTest.Domain.Models;
using PracticalTest.External.Contracts;
using PracticalTest.External.Constants;

namespace PracticalTest.External.Services
{
   public class WrapperReqResService : IWrapperReqResService
   {
      private readonly IWrapperApiService _wrapperApiService;

      public WrapperReqResService(IWrapperApiService wrapperApiService)
      {
         _wrapperApiService = wrapperApiService;
      }

      /// <summary>
      /// Get list of users by page number
      /// </summary>
      /// <param name="pageNumber"></param>
      /// <returns></returns>
      public async Task<UserList> GetUsersAsync(int pageNumber)
      {
         return await _wrapperApiService.GetAsync<UserList>(WrapperApi.ReqRes, $"users?page={pageNumber}", "ERROR_RETRIEVING_USERS");
      }

      /// <summary>
      /// Get details of user by id
      /// </summary>
      /// <param name="userId"></param>
      /// <returns></returns>
      public async Task<User> GetUserAsync(int userId)
      {
         return await _wrapperApiService.GetAsync<User>(WrapperApi.ReqRes, $"users/{userId}", "ERROR_RETRIEVING_USER");
      }
   }
}
