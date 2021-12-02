using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIO.Infrastructure;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.TranslationOptions.Queries
{
    public class GetTranslationOptionsQuery : Query<GetTranslationOptionsQueryResult>
    {
        public GetTranslationOptionsQuery(CorrelationId? correlationId, Actor actor) : base(correlationId, actor)
        {
        }
    }
}
