using Microsoft.Extensions.Logging;
using Moq;
using PracticalTest.Domain.Models;
using PracticalTest.External.Contracts;
using PracticalTest.Service;

namespace PracticalTest.Tests;

public class UserServiceTest
{
   [Fact]
   public async Task GetUserAsync_ReturnsValid()
   {
      var loggerMock = new Mock<ILogger<UserService>>();

      var mockWrapperReqResService = new Mock<IWrapperReqResService>();

      var expectedUser = new User { Data = new Data { 
         Id = 1,
         FirstName = "Test",
      }};

      mockWrapperReqResService.Setup(r => r.GetUserAsync(1)).ReturnsAsync(expectedUser);

      var service = new UserService(loggerMock.Object, mockWrapperReqResService.Object);

      var result = await service.GetUserAsync(1);

      Assert.NotNull(result);
      Assert.Equal(1, result.Data.Id);
      Assert.Equal("Test", result.Data.FirstName);
   }  
}