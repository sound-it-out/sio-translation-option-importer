using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SIO.Domain.TranslationOptions.Commands;
using SIO.Domain.TranslationOptions.Queries;
using SIO.Infrastructure;
using SIO.Infrastructure.Commands;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.TranslationOptions.Services
{
    internal sealed class TranslationOptionsImporter : IHostedService
    {
        private Task _executingTask;
        private CancellationTokenSource StoppingCts { get; set; }
        private readonly IServiceScope _scope;
        private readonly ILogger<TranslationOptionsImporter> _logger;
        private readonly IOptionsSnapshot<TranslationOptionsImporterOptions> _options;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public TranslationOptionsImporter(IServiceScopeFactory serviceScopeFactory,
            ILogger<TranslationOptionsImporter> logger)
        {
            if (serviceScopeFactory == null)
                throw new ArgumentNullException(nameof(serviceScopeFactory));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _scope = serviceScopeFactory.CreateScope();
            _logger = logger;
            _options = _scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<TranslationOptionsImporterOptions>>();
            _commandDispatcher = _scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
            _queryDispatcher = _scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(TranslationOptionsImporter)}.{nameof(StartAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            _logger.LogInformation($"{nameof(TranslationOptionsImporter)} starting");
            StoppingCts = new();

            _executingTask = ExecuteAsync(StoppingCts.Token);

            _logger.LogInformation($"{nameof(TranslationOptionsImporter)} started");

            if (_executingTask.IsCompleted)
                return _executingTask;

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(TranslationOptionsImporter)}.{nameof(StopAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            _logger.LogInformation($"{nameof(TranslationOptionsImporter)} stopping");

            if (_executingTask == null)
                return;

            try
            {
                StoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
                _logger.LogInformation($"{nameof(TranslationOptionsImporter)} stopped");
            }
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(TranslationOptionsImporter)}.{nameof(ExecuteAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var correlationId = CorrelationId.New();
                    var translationOptions = await _queryDispatcher.DispatchAsync(new GetTranslationOptionsQuery(correlationId, Actor.Unknown));

                    foreach (var option in translationOptions.TranslationOptions)
                    {
                        await _commandDispatcher.DispatchAsync(new ImportTranslationOptionCommand(
                            subject: option.Subject,
                            correlationId: correlationId,
                            version: 0,
                            Actor.Unknown,
                            translationType: option.TranslationType
                        ));
                    }

                    await Task.Delay(_options.Value.Interval);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, $"Process '{nameof(TranslationOptionsImporter)}.{nameof(ExecuteAsync)}' failed due to an unexpected error. See exception details for more information.");
                    break;
                }
            }
        }
    }
}
