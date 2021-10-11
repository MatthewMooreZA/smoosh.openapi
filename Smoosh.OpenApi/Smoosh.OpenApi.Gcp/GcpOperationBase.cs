using System.Collections.Generic;
using Microsoft.OpenApi.Interfaces;
using Smoosh.OpenApi.Gcp.Models;

namespace Smoosh.OpenApi.Gcp
{
    public abstract class GcpOperationBase
    {
        public const string GoogleBackendExtension = "x-google-backend";

        public bool TryAddGoogleBackendExtension(IDictionary<string, IOpenApiExtension> extensions, GoogleBackendExtension googleBackendExtension)
        {
            if (extensions.ContainsKey(GoogleBackendExtension))
            {
                extensions.Remove(GoogleBackendExtension);
            }
            
            extensions.Add(GoogleBackendExtension, googleBackendExtension.ToOpenApiObject());
            return true;
        }



    }
}
