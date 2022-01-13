using SIO.Infrastructure.Domain;
using SIO.IntegrationEvents.Documents;
using SIO.IntegrationEvents.TranslationOptions;

namespace SIO.Domain.TranslationOptions.Aggregates
{
    public sealed class TranslationOption : Aggregate<TranslationOptionState>
    {
        public TranslationOption(TranslationOptionState state) : base(state)
        {
            Handles<TranslationOptionImported>(Handle);
        }

        public override TranslationOptionState GetState() => new TranslationOptionState(_state);

        public void Import(string subject, TranslationType translationType)
        {
            Apply(new TranslationOptionImported(
                subject: subject,
                version: Version + 1,
                translationType: translationType
            ));
        }

        private void Handle(TranslationOptionImported @event)
        {
            Id = @event.Subject;
            _state.TranslationType = @event.TranslationType;
            Version = @event.Version;
        }
    }
}
