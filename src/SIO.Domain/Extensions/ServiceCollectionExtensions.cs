using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIO.Domain.TranslationOptions.CommandHandlers;
using SIO.Domain.TranslationOptions.Commands;
using SIO.Domain.TranslationOptions.Queries;
using SIO.Domain.TranslationOptions.QueryHandlers;
using SIO.Domain.TranslationOptions.Services;
using SIO.Infrastructure.Commands;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICommandHandler<ImportTranslationOptionCommand>, ImportTranslationOptionCommandHandler>();
            services.AddScoped<IQueryHandler<GetTranslationOptionsQuery, GetTranslationOptionsQueryResult>, GetTranslationOptionsQueryHandler>();
            services.AddHostedService<TranslationOptionsImporter>();
            services.Configure<TranslationOptionsImporterOptions>(o => o.Interval = (int)TimeSpan.FromDays(1).TotalMilliseconds);
            return services;
        }
    }
}
