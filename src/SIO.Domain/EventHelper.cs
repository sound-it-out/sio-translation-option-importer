using System;
using System.Linq;
using SIO.Domain.Documents.Events;
using SIO.Domain.TranslationOptions.Events;

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
