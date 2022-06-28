using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
//using NutritionList.Model;
namespace NutritionList
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            string name;

            var builder = new ConfigurationBuilder()
                   .AddJsonFile($"appsettings.json", true, true);

            Console.WriteLine("Enter the product name to get the details");
            name = Console.ReadLine();

            var config = builder.Build();
            var appID = config["Setting:x-app-id"];
            var appKey = config["Setting:x-app-key"];

            //Dummy post body for a name
            var data = new
            {
                query = name,
            };

            var json = JsonConvert.SerializeObject(data);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            //can make it configurable too in app setting //Assuming as per the documents
            var baseAddress = "https://trackapi.nutritionix.com/v2/natural/nutrients"; 
            using (var client = new HttpClient())
            {
                var uriBuilder = new UriBuilder(baseAddress);
                
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-app-key", appID);
                client.DefaultRequestHeaders.Add("x-app-id", appKey);

                //Post Method Giving unauthorzied with above keys
                HttpResponseMessage response = await client.PostAsync(uriBuilder.ToString(), content : content);
                if (response.IsSuccessStatusCode)
                {
                    //Can do mapping via automapper if i would be knowing all response model
                    // Product product = await response.Content.ReadAsAsync<Product>();
                    Console.WriteLine("Can print the reponse here after mapping");
                    Console.WriteLine("-----END--------");
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }
        }
    }
}
