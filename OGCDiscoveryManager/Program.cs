using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OGCDiscoveryManager
{
    public class Program
    {
        private static readonly AutoResetEvent waitHandle = new AutoResetEvent(false);

        public static void Main(string[] args)
        {
            string connstr = Environment.GetEnvironmentVariable("CONNECTION_STR");
            if (connstr == null || connstr == "")
            {
                System.Console.WriteLine("Warning:Missing CONNECTION_STR env variable.");
            }
            else IO.Swagger.settings.ConnectionString = connstr;
            DiscoveryProcess disc = new DiscoveryProcess();
            disc.start();

            CreateWebHostBuilder(args).Build().Run();
            while (true)
                System.Threading.Thread.Sleep(500);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
