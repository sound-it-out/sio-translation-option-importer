using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIO.Domain.TranslationOptions.Aggregates;
using SIO.Domain.TranslationOptions.Commands;
using SIO.Infrastructure.Commands;
using SIO.Infrastructure.Domain;
using SIO.Infrastructure.Events;

namespace SIO.Domain.TranslationOptions.CommandHandlers
{
    internal sealed class ImportTranslationOptionCommandHandler : ICommandHandler<ImportTranslationOptionCommand>
    {
        private readonly ILogger<ImportTranslationOptionCommandHandler> _logger;
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IAggregateFactory _aggregateFactory;

        public ImportTranslationOptionCommandHandler(ILogger<ImportTranslationOptionCommandHandler> logger,
            IAggregateRepository aggregateRepository,
            IAggregateFactory aggregateFactory)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));
            if (aggregateFactory == null)
                throw new ArgumentNullException(nameof(aggregateFactory));

            _logger = logger;
            _aggregateRepository = aggregateRepository;
            _aggregateFactory = aggregateFactory;
        }

        public async Task ExecuteAsync(ImportTranslationOptionCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(ImportTranslationOptionCommandHandler)}.{nameof(ExecuteAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var aggregate = await _aggregateRepository.GetAsync<TranslationOption, TranslationOptionState>(command.Subject, cancellationToken);

            if (aggregate != null)
            {
                _logger.LogInformation($"{nameof(ImportTranslationOptionCommandHandler)}.{nameof(ExecuteAsync)} - Translation option with type: {command.TranslationType} and subject: {command.Subject} already exists and has not been imported");
                return;
            }                

            aggregate = _aggregateFactory.FromHistory<TranslationOption, TranslationOptionState>(Enumerable.Empty<IEvent>());

            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            aggregate.Import(
                subject: command.Subject,
                translationType: command.TranslationType
            );

            await _aggregateRepository.SaveAsync(aggregate, command, cancellationToken: cancellationToken);
        }
    }
}
