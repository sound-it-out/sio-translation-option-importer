using SIO.IntegrationEvents.Documents;

namespace SIO.Domain.TranslationOptions.Services
{
    public sealed record TranslationOption(string Subject, TranslationType TranslationType);
}
