using System;
using System.Linq;
using Microsoft.OpenApi.Models;
using OpenApi.Smoosh.Common.Extensions;

namespace OpenApi.Smoosh.Common.Operations
{
    public class RenameComponentsOperation : IOperation
    {
        private readonly string _suffix;

        public RenameComponentsOperation(string suffix, int ordinal)
        {
            _suffix = suffix;
            Ordinal = ordinal;
        }

        public RenameComponentsOperation(int ordinal) 
            : this(Guid.NewGuid().ToString(), ordinal)
        {
        }

        public int Ordinal { get; }

        public void Apply(OpenApiDocument document)
        {
            RenameSchemas(document);
            RenameOperations(document);
        }

        private void RenameSchemas(OpenApiDocument document)
        {
            var schemaMap = document.Components.Schemas
                .ToDictionary(k => k.Key, v => $"{v.Key}_{_suffix}");

            foreach (var mapAction in schemaMap)
            {
                var item = document.Components.Schemas.Rekey(mapAction.Key, mapAction.Value);

                if (item?.Reference?.Type == ReferenceType.Schema)
                {
                    item.Reference.Id = schemaMap[item.Reference.Id];
                }
            }
        }

        private void RenameOperations(OpenApiDocument document)
        {
            var operations = document.Paths.Values.SelectMany(p => p.Operations).Select(o => o.Value);

            foreach (var operation in operations)
            {
                operation.OperationId = $"{operation.OperationId}_{_suffix}";
            }
        }
    }
}
