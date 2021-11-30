using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TodoStudy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                    .UseKestrel()
                    .UseStartup<Startup>()
                    .ConfigureKestrel((context, serverOptions) =>
                    {
                        serverOptions.ListenAnyIP(5000);
                    });
                //.UseKestrel(options =>
                //{
                //    options.AddServerHeader = false;
                //    options.ListenAnyIP(5000);
                //})
                //.UseStartup<Startup>();
    }
}
