using SIO.Domain.TranslationOptions.Projections;
using SIO.Domain.TranslationOptions.Projections.Managers;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.EntityFrameworkCore.Extensions;

namespace SIO.Domain.Extensions
{
    public static class EntityFrameworkCoreStoreProjectorOptionsExtensions
    {
        public static void WithDomainProjections(this EntityFrameworkCoreStoreProjectorOptions options)
            => options.WithProjection<TranslationOption, TranslationOptionProjectionManager, SIOStoreDbContext>(o => o.Interval = 5000);
    }
}
