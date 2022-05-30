using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;

namespace Smoosh.OpenApi.Common.Operations
{
    public class MergeOperation : IOperation
    {
        private readonly OpenApiDocument _other;
        private readonly string _mergeSuffix;
        public MergeOperation(OpenApiDocument other, int mergeNumber)
        {
            _other = other;
            _mergeSuffix = $"_{mergeNumber}";

        }

        public void Apply(OpenApiDocument document)
        {
            if (_other == null) return;

            var operationIds = new HashSet<string>(
                document.Paths.Values.SelectMany(p => p.Operations.Values.Select(o => o.OperationId)));

            foreach (var path in _other.Paths)
            {
                EnsureOperationsDoNotCollide(path, operationIds);

                if (!document.Paths.ContainsKey(path.Key))
                {
                    document.Paths.Add(path.Key, path.Value);
                    continue;
                }

                throw new NotImplementedException("Handle conflicts");
            }

            foreach (var scheme in _other.Components.Schemas)
            {
                // todo: evaluate schema elements to reduce duplication
                scheme.Value.Reference.Id += _mergeSuffix;
                document.Components.Schemas.Add(scheme.Key + _mergeSuffix, scheme.Value);
            }
        }

        private void EnsureOperationsDoNotCollide(KeyValuePair<string, OpenApiPathItem> path, HashSet<string> operationIds)
        {
            foreach (var operation in path.Value.Operations)
            {
                if (operationIds.Contains(operation.Value.OperationId))
                {
                    operation.Value.OperationId += _mergeSuffix;
                }
            }
        }
    }
}
