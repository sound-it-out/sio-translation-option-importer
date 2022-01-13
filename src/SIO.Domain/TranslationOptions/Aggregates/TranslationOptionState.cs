using System;
using SIO.Infrastructure.Domain;
using SIO.IntegrationEvents.Documents;

namespace SIO.Domain.TranslationOptions.Aggregates
{
    public sealed class TranslationOptionState : IAggregateState
    {
        public string Subject { get; set; }
        public TranslationType TranslationType { get; set; }

        public TranslationOptionState() { }
        public TranslationOptionState(TranslationOptionState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            Subject = state.Subject;
            TranslationType = state.TranslationType;
        }
    }
}
