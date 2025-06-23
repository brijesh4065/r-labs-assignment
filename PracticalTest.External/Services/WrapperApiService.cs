using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Retry;
using PracticalTest.Domain.Models;
using PracticalTest.External.Constants;
using PracticalTest.External.Contracts;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace PracticalTest.External.Services
{
   public class WrapperApiService : IWrapperApiService
   {
      private readonly IHttpClientFactory _httpClientFactory;
      private JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

      private readonly IMemoryCache _cache;
      private readonly MemoryCacheEntryOptions _cacheOptions;

      public WrapperApiService(IHttpClientFactory httpClientFactory, IMemoryCache cache, MemoryCacheEntryOptions cacheOptions)
      {
         _httpClientFactory = httpClientFactory;
         _cache = cache;
         _cacheOptions = cacheOptions;
      }

      /// <summary>
      /// Generic method to retrieve data from a remote service
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="wrapperApi"></param>
      /// <param name="url"></param>
      /// <param name="errorMessage"></param>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
      public async Task<T> GetAsync<T>(WrapperApi wrapperApi, string? url, string errorMessage)
      {
         if (_cache.TryGetValue(url, out string cachedData))
         {
            if (!string.IsNullOrEmpty(cachedData))
            {
               return JsonSerializer.Deserialize<T>(cachedData, jsonSerializerOptions);
            }
         }

         var client = GetHttpClient(wrapperApi);

         AsyncRetryPolicy<HttpResponseMessage> retryPolicy = GetRetryPolicy();

         var response = await retryPolicy.ExecuteAsync(() => client.GetAsync(url));

         var responseString = await response.Content.ReadAsStringAsync();

         if (response.IsSuccessStatusCode)
         {
            _cache.Set(url, responseString, _cacheOptions);
            var result = JsonSerializer.Deserialize<T>(responseString, jsonSerializerOptions);
            return result;
         }
         else if (response.StatusCode == HttpStatusCode.NotFound)
         {
            throw new Exception("Resource not found");
         }
         else if (response.StatusCode == HttpStatusCode.BadRequest)
         {
            throw new Exception(responseString);
         }
         else
         {
            throw new Exception(errorMessage);
         }
      }

      private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
      {
         return Policy
                     .Handle<HttpRequestException>()
                     .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                     .WaitAndRetryAsync(3, retryAttempt =>
                         TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                         (response, timespan, retryCount, context) =>
                         {
                            Console.WriteLine($"Retry {retryCount} after {timespan.TotalSeconds} sec due to: {response.Exception?.Message ?? response.Result.StatusCode.ToString()}");
                         });
      }

      private HttpClient GetHttpClient(WrapperApi wrapperApi)
      {
         var clientName = wrapperApi switch
         {
            WrapperApi.ReqRes => "ReqResApi",
            _ => ""
         };
         var client = _httpClientFactory.CreateClient(clientName);
         client.Timeout = new TimeSpan(1, 0, 0);
         return client;
      }
   }
}
