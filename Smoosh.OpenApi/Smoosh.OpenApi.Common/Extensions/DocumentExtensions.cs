using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;

namespace Smoosh.OpenApi.Common.Extensions
{
    public static class DocumentExtensions
    {
        public static IEnumerable<OpenApiReference> ReferencesOf(this OpenApiDocument document, ReferenceType referenceType)
        {
            return document.Paths.Values
                .SelectMany(p => p.Operations)
                .SelectMany(p => p.Value.Responses.Values)
                .SelectMany(p => p.Content.Values)
                .Select(x => x?.Schema?.Reference)
                .Where(r => r != null && r.Type == referenceType);
        }
    }
}
