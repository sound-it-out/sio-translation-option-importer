using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIO.Domain.TranslationOptions.Services;
using SIO.Google.TranslationOptions.Services;

namespace SIO.Google.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGoogleTranslations(this IServiceCollection services, IConfiguration configuration)
        {
            if (!string.IsNullOrEmpty(configuration.GetValue<string>("Google:Credentials:type")))
                services.AddGoogleConfiguration(configuration).AddGoogleTranslations();

            return services;
        }

        private static IServiceCollection AddGoogleTranslations(this IServiceCollection services)
        {
            services.AddScoped<ITranslationOptionsRetriever, GoogleTranslationOptionsRetriever>();
            return services;
        }

        public static IServiceCollection AddGoogleConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleCredentialOptions>(o =>
            {
                o.Type = configuration.GetValue<string>("Google:Credentials:type");
                o.ProjectId = configuration.GetValue<string>("Google:Credentials:project_id");
                o.PrivateKeyId = configuration.GetValue<string>("Google:Credentials:private_key_id");
                o.PrivateKey = configuration.GetValue<string>("Google:Credentials:private_key");
                o.ClientEmail = configuration.GetValue<string>("Google:Credentials:client_email");
                o.ClientId = configuration.GetValue<string>("Google:Credentials:client_id");
                o.AuthUri = configuration.GetValue<string>("Google:Credentials:auth_uri");
                o.TokenUri = configuration.GetValue<string>("Google:Credentials:token_uri");
                o.AuthProviderX509CertUrl = configuration.GetValue<string>("Google:Credentials:auth_provider_x509_cert_url");
                o.ClientX509CertUrl = configuration.GetValue<string>("Google:Credentials:client_x509_cert_url");
            });
            return services;
        }
    }
}
