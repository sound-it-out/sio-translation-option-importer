using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SIO.Domain.TranslationOptions.Services
{
    public interface ITranslationOptionsRetriever
    {
        Task<IEnumerable<TranslationOption>> GetTranslationsAsync(CancellationToken cancellationToken = default);
    }
}
