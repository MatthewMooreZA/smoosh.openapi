using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using OpenApi.Smoosh.Common.Operations;
using OpenApi.Smoosh.Gcp.Models;

namespace OpenApi.Smoosh.Gcp.Operations
{
    public class CloudRunOperation : GcpOperationBase, IOperation
    {
        public string CloudRunUrl { get; set; }
        public Protocols Protocol { get; set; }
        public Security Security { get; set; }
        public TimeSpan Timeout { get; set; }
        public List<Predicate<string>> PathFilter { get; set; } = new List<Predicate<string>>();
        public int Ordinal { get; }
        public void Apply(OpenApiDocument document)
        {
            Security.Apply(document);
            foreach (var path in document.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    if (!TryAddGoogleBackendExtension(operation.Value.Extensions, CloudRunUrl))
                    {
                        throw new Exception("Failed to map to Google Backend");
                    }

                    Security.Apply(document, operation.Value);

                }
            }
        }
    }
}
