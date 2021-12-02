using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIO.Domain.TranslationOptions.Queries;
using SIO.Domain.TranslationOptions.Services;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.TranslationOptions.QueryHandlers
{
    internal class GetTranslationOptionsQueryHandler : IQueryHandler<GetTranslationOptionsQuery, GetTranslationOptionsQueryResult>
    {
        private readonly ILogger<GetTranslationOptionsQueryHandler> _logger;
        private readonly IEnumerable<ITranslationOptionsRetriever> _translationOptionsRetrievers;

        public GetTranslationOptionsQueryHandler(ILogger<GetTranslationOptionsQueryHandler> logger,
            IEnumerable<ITranslationOptionsRetriever> translationOptionsRetrievers)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (translationOptionsRetrievers == null)
                throw new ArgumentNullException(nameof(translationOptionsRetrievers));

            _logger = logger;
            _translationOptionsRetrievers = translationOptionsRetrievers;
        }

        public async Task<GetTranslationOptionsQueryResult> RetrieveAsync(GetTranslationOptionsQuery query, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(GetTranslationOptionsQueryHandler)}.{nameof(RetrieveAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var translationOptions = new ConcurrentBag<TranslationOption>();

            await Task.WhenAll(_translationOptionsRetrievers.Select(tor => RetrieveOptionsAsync(tor, translationOptions, cancellationToken)));

            return new GetTranslationOptionsQueryResult(translationOptions.OrderBy(to => to.TranslationType));
        }

        private async Task RetrieveOptionsAsync(ITranslationOptionsRetriever translationOptionsRetriever, ConcurrentBag<TranslationOption> translationOptions, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(GetTranslationOptionsQueryHandler)}.{nameof(RetrieveOptionsAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var options = await translationOptionsRetriever.GetTranslationsAsync(cancellationToken);

            foreach (var option in options)
                translationOptions.Add(option);
        }
    }
}
