using PracticalTest.External.Constants;

namespace PracticalTest.External.Contracts
{
    public interface IWrapperApiService
    {
        Task<T> GetAsync<T>(WrapperApi wrapperApi, string? url, string errorMessage);
    }
}
