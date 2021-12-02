﻿using SIO.Domain.Documents.Events;
using SIO.Infrastructure;
using SIO.Infrastructure.Commands;

namespace SIO.Domain.TranslationOptions.Commands
{
    public sealed class ImportTranslationOptionCommand : Command
    {
        public TranslationType TranslationType { get; }
        public ImportTranslationOptionCommand(string subject, CorrelationId? correlationId, int version, Actor actor, TranslationType translationType) : base(subject, correlationId, version, actor)
        {
            TranslationType = translationType;
        }
    }
}
