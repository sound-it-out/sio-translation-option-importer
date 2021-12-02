using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SIO.Infrastructure.EntityFrameworkCore.Extensions;
using SIO.TranslationOption.Importer.Extensions;

namespace SIO.TranslationOption.Importer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var env = host.Services.GetRequiredService<IHostEnvironment>();

            if (env.IsDevelopment())
                await host.RunProjectionMigrationsAsync();

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => services.AddInfrastructure(hostContext.Configuration));
    }
}
