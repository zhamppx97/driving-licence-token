using Coravel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace driving_licence_token
{
    class Program
    {
        static void Main(string[] args)
        {
            _ = new Blockchain();

            IHost host = CreateHostBuilder(args).Build();
            host.Services.UseScheduler(scheduler =>
            {
                scheduler.Schedule<BlockJob>().EveryFifteenSeconds();
            });
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5001, listenOptions => listenOptions.Protocols = HttpProtocols.Http1); //webapi
                options.ListenAnyIP(5002, listenOptions => listenOptions.Protocols = HttpProtocols.Http2); //grpc
            });
            webBuilder.UseStartup<Startup>();
        });
    }
}
