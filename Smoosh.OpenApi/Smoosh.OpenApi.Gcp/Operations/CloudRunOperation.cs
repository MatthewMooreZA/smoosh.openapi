using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Smoosh.OpenApi.Common.Operations;
using Smoosh.OpenApi.Gcp.Models;

namespace Smoosh.OpenApi.Gcp.Operations
{
    public class CloudRunOperation : GcpOperationBase, IOperation
    {
        public string CloudRunUrl { get; set; }
        public Protocols Protocol { get; set; }
        public Security Security { get; set; }
        public TimeSpan? Timeout { get; set; }
        public List<Predicate<string>> PathFilter { get; set; } = new List<Predicate<string>>();

        public IReadOnlyDictionary<string, string> RemappedPathsLookup = new Dictionary<string, string>();
        public void Apply(OpenApiDocument document)
        {
            Security.Apply(document);
            foreach (var path in document.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    var backendExtension = new GoogleBackendExtension();

                    if (RemappedPathsLookup.ContainsKey(path.Key))
                    {
                        var originalPath = RemappedPathsLookup[path.Key];

                        var uri = new Uri(CloudRunUrl);
                        var combined = new Uri(uri, originalPath);
                        backendExtension.SetRemappedUrl(combined.ToString());
                    }
                    else
                    {
                        backendExtension.SetUrl(CloudRunUrl);
                    }


                    backendExtension.SetProtocol(Protocol);
                    if (Timeout != null)
                    {
                        backendExtension.SetTimeout(Timeout.Value);
                    }

                    if (!TryAddGoogleBackendExtension(operation.Value.Extensions, backendExtension))
                    {
                        throw new Exception("Failed to map to Google Backend");
                    }

                    Security.Apply(document, operation.Value);

                }
            }
        }
    }
}
