using System;
using System.Linq;
using Microsoft.OpenApi.Models;
using Smoosh.OpenApi.Common.Extensions;

namespace Smoosh.OpenApi.Common.Operations
{
    public class AdjustPathsOperation : IOperation
    {
        private readonly Func<string, string> _transform;

        public AdjustPathsOperation(Func<string, string> transform)
        {
            _transform = transform;
        }

        public void Apply(OpenApiDocument document)
        {
            var mapping = document.Paths
                .Select(p => new { old = p.Key, @new = _transform.Invoke(p.Key) })
                .Where(m => m.old != m.@new)
                .ToList();

            foreach (var map in mapping)
            {
                document.Paths.Rekey(map.old, map.@new);
                OnPathAdjustment?.Invoke(map.old, map.@new, document);
            }
        }

        protected internal Action<string, string, OpenApiDocument> OnPathAdjustment { get; set; }
    }
}
