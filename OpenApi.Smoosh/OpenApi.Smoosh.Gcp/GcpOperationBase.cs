using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using OpenApi.Smoosh.Gcp.Models;

namespace OpenApi.Smoosh.Gcp
{
    public abstract class GcpOperationBase
    {
        public const string GoogleBackendExtension = "x-google-backend";

        public bool TryAddGoogleBackendExtension(IDictionary<string, IOpenApiExtension> extensions, string url, Protocols protocol, TimeSpan? timeout)
        {
            if (extensions.ContainsKey(GoogleBackendExtension))
            {
                extensions.Remove(GoogleBackendExtension);
            }

            var backendProps = new OpenApiObject
            {
                {"address", new OpenApiString(url)},
                {"protocol", new OpenApiString(ProtocolToString(protocol))}
            };

            if (timeout.HasValue)
            {
                backendProps.Add("deadline", new OpenApiDouble(timeout.Value.TotalSeconds));
            }

            extensions.Add(GoogleBackendExtension, backendProps);
            return true;
        }

        private string ProtocolToString(Protocols protocol)
        {
            return protocol == Protocols.Http2 ? "h2" : "http/1.1";
        }

    }
}
