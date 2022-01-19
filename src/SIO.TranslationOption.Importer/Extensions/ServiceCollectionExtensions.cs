using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIO.AWS.Extensions;
using SIO.Domain;
using SIO.Domain.Extensions;
using SIO.Google.Extensions;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.EntityFrameworkCore.Extensions;
using SIO.Infrastructure.EntityFrameworkCore.SqlServer.Extensions;
using SIO.Infrastructure.Extensions;
using SIO.Infrastructure.Serialization.Json.Extensions;
using SIO.Infrastructure.Serialization.MessagePack.Extensions;

namespace SIO.TranslationOption.Importer.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDomain(configuration)
                .AddAWSTranslations()
                .AddGoogleTranslations(configuration)
                .AddSIOInfrastructure()
                .AddEntityFrameworkCoreSqlServer(options =>
                {
                    options.AddStore<SIOStoreDbContext>(configuration.GetConnectionString("Store"), o => o.MigrationsAssembly($"{nameof(SIO)}.{nameof(Migrations)}"));
                    options.AddProjections(configuration.GetConnectionString("Projection"), o => o.MigrationsAssembly($"{nameof(SIO)}.{nameof(Migrations)}"));
                })
                .AddEntityFrameworkCoreStoreProjector(options => options.WithDomainProjections())
                .AddEvents(o => o.Register(EventHelper.AllEvents))
                .AddCommands()
                .AddQueries()
                .AddEventBus(configuration)
                .AddJsonSerializers();

            return services;
        }
    }
}
