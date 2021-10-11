using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using OpenApi.Smoosh.Gcp.Models;

namespace OpenApi.Smoosh.Gcp
{
    public abstract class GcpOperationBase
    {
        public const string GoogleBackendExtension = "x-google-backend";

        public bool TryAddGoogleBackendExtension(IDictionary<string, IOpenApiExtension> extensions, string url)
        {
            if (extensions.ContainsKey(GoogleBackendExtension)) return false;
            extensions.Add(GoogleBackendExtension, new OpenApiObject
            {
                {"address", new OpenApiString(url)}
            });
            return true;
        }

    }
}
