using System.Text.Json.Serialization;

namespace PracticalTest.Domain.Models
{
   public class User
   {
      public Data Data { get; set; }
      public Support Support { get; set; }
   }

   public class UserList
   {
      public int Page { get; set; }

      [JsonPropertyName("per_page")]
      public int PerPage { get; set; }

      public int Total { get; set; }


      [JsonPropertyName("total_pages")]
      public int TotalPages { get; set; }

      public List<Data> Data { get; set; }

      public Support Support { get; set; }
   }


   public class Data
   {
      public int Id { get; set; }
      public string Email { get; set; }

      [JsonPropertyName("first_name")]
      public string FirstName { get; set; }

      [JsonPropertyName("last_name")]
      public string LastName { get; set; }
      
      public string Avatar { get; set; }
   }

   public class Support
   {
      public string Url { get; set; }
      public string Text { get; set; }
   }

}
