using Microsoft.Extensions.DependencyInjection;
using SIO.AWS.TranslationOptions.Services;
using SIO.Domain.TranslationOptions.Services;

namespace SIO.AWS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAWSTranslations(this IServiceCollection services)
        {
            services.AddSingleton<ITranslationOptionsRetriever, AWSTranslationOptionsRetriever>();
            return services;
        }
    }
}
