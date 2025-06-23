using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalTest.Client.Model
{
   public class AppSettings
   {
      public ReqResApiSettings ReqResApiSettings { get; set; }
      public MemoryCacheSettings MemoryCacheSettings { get; set; }
   }

   public class ReqResApiSettings
   {
      public string Url { get; set; }
      public string Key { get; set; }
   }

   public class MemoryCacheSettings
   {
      public int ExpirationInMinute { get; set; }
   }
}
