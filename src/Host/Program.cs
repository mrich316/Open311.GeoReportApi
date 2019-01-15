using System;
using Microsoft.AspNetCore;
using Serilog;

namespace Host
{
    using System.IO;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, "Application shutdown...");
                Log.CloseAndFlush();
                Environment.ExitCode = -1;
            }
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog(CreateLogger)
                .UseStartup<Startup>();

        public static void CreateLogger(WebHostBuilderContext context, LoggerConfiguration logger)
        {
            logger.WriteTo.Console()
                .Enrich.FromLogContext();
        }
    }
}
