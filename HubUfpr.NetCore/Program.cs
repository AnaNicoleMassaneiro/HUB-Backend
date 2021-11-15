using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace HubUfpr.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:4000", "http://192.168.18.70:4000")
                .UseStartup<Startup>();
    }
}