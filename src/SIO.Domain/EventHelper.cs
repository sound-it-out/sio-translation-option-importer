using System;
using SIO.IntegrationEvents.Documents;
using SIO.IntegrationEvents.TranslationOptions;

namespace SIO.Domain
{
    public static class EventHelper
    {
        public static Type[] AllEvents = new Type[]
        {
            typeof(DocumentUploaded),
            typeof(TranslationOptionImported)
        };
    }
}
