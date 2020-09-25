using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace dotnet_new_angular
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .ConfigureKestrel((context, serverOptions) =>
                        {
                            serverOptions.AddServerHeader = false;
                            if (context.HostingEnvironment.IsDevelopment())
                            {
                                serverOptions.ListenLocalhost(51384, listenOptions => listenOptions
                                    .UseHttps()

                                    // This avoids ERR_HTTP2_INADEQUATE_TRANSPORT_SECURITY when developing locally by forcing http1.
                                    // Solution inspired by https://github.com/aspnet/AspNetCore/issues/14350#issuecomment-542374687
                                    .Protocols = HttpProtocols.Http1);
                            }
                        });
                });
    }
}