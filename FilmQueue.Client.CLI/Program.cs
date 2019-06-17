using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace FilmQueue.Client.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var response = httpClient.PostAsync(
                    "https://localhost:44312/connect/token",
                    new StringContent(
                        "grant_type=client_credentials&scope=api.read&client_id=cli&client_secret=<secret_password>",
                        Encoding.UTF8,
                        "application/x-www-form-urlencoded")
                ).Result;

                var content = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(content);
                Console.ReadLine();
            }
        }
    }
}
