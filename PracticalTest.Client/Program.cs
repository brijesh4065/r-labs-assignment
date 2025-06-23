using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Memory;
using PracticalTest.Client.Model;
using PracticalTest.Domain.Models;
using PracticalTest.External.Contracts;
using PracticalTest.External.Services;
using PracticalTest.Service;
using PracticalTest.Service.Contracts;

namespace PracticalTest.Client
{
   public class Program
   {
      public static void Main(string[] args)
      {
         CreateHostBuilder(args).Build().Run();
      }

      public static IHostBuilder CreateHostBuilder(string[] args)
      {
         return Host.CreateDefaultBuilder(args)
         .ConfigureAppConfiguration((hostingContext, config) =>
         {
            PopulateAppConfiguration(config);
         })
         .ConfigureServices((hostContext, services) =>
         {
            AppSettings appSettings;

            appSettings = GetConfigurationDetails(hostContext);

            ConfigureServices(services, appSettings);
            ConfigureHttpClients(services, appSettings);
            ConfigureMemoryCache(services, appSettings);

            RunApp(services);
         });
      }

      private static void ConfigureMemoryCache(IServiceCollection services, AppSettings appSettings)
      {
         services.AddMemoryCache();

         services.AddSingleton(new MemoryCacheEntryOptions
         {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(appSettings.MemoryCacheSettings.ExpirationInMinute)
         });
      }

      private static void PopulateAppConfiguration(IConfigurationBuilder config)
      {
         var configBuilder = new ConfigurationBuilder()
                                 .Build();

         var builtConfig = config.Build();

         config.AddJsonFile("appsecrets.json", optional: false, reloadOnChange: true);
      }

      private static AppSettings GetConfigurationDetails(HostBuilderContext hostContext)
      {
         ReqResApiSettings reqResApiSettings;
         MemoryCacheSettings memoryCacheSettings;

         var config = hostContext.Configuration;
         reqResApiSettings = config.GetSection("ReqResApiSettings").Get<ReqResApiSettings>();
         memoryCacheSettings = config.GetSection("MemoryCacheSettings").Get<MemoryCacheSettings>();

         var appSettings = new AppSettings()
         {
            ReqResApiSettings = new ReqResApiSettings
            {
               Key = reqResApiSettings.Key,
               Url = reqResApiSettings.Url
            },
            MemoryCacheSettings = new MemoryCacheSettings
            {
               ExpirationInMinute = memoryCacheSettings.ExpirationInMinute
            },
         };
         return appSettings;
      }

      private static void ConfigureServices(IServiceCollection services, AppSettings appSettings)
      {
         services.AddSingleton(s => appSettings);
         services.AddHttpClient();
         services.AddScoped<IWrapperApiService, WrapperApiService>();
         services.AddScoped<IWrapperReqResService, WrapperReqResService>();
         services.AddScoped<IUserService, UserService>();
      }

      private static void ConfigureHttpClients(IServiceCollection services, AppSettings appSettings)
      {
         services.AddHttpClient("ReqResApi", c =>
         {
            c.BaseAddress = new Uri(appSettings.ReqResApiSettings.Url);
            c.DefaultRequestHeaders.Add("X-API-Key", appSettings.ReqResApiSettings.Key);
         });
      }

      private static async void RunApp(IServiceCollection services)
      {
         var serviceProvider = services.BuildServiceProvider();
         var userService = serviceProvider.GetRequiredService<IUserService>();
         await GetUser(userService);
         await GetUsers(userService);

         //Uncomment the line below to verify cached results
         //Thread.Sleep(1000);
         //await GetUser(userService);
         //await GetUsers(userService);
      }

      private static async Task GetUser(IUserService userService)
      {
         try
         {
            var user = await userService.GetUserAsync(1);
            LogData(user.Data);
         }
         catch (Exception)
         {
            Console.WriteLine("Error occurred while getting the user information.");
         }
      }

      private static async Task GetUsers(IUserService userService)
      {
         try
         {
            var users = await userService.GetUsersAsync();

            foreach (var user in users)
            {
               LogData(user);
            }
         }
         catch (Exception)
         {
            Console.WriteLine("Error occurred while getting the users information.");
         }
      }

      private static void LogData(Data user)
      {
         Console.WriteLine("****** User Details ***********");
         Console.WriteLine("Id: " + user.Id);
         Console.WriteLine("FirstName: " + user.FirstName);
         Console.WriteLine("LastName: " + user.LastName);
         Console.WriteLine("********************************");
      }
   }
}
