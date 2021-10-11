using System;
using Microsoft.OpenApi.Models;

namespace OpenApi.Smoosh.Common.Operations
{
    public class MergeOperation : IOperation
    {
        private readonly OpenApiDocument _other;
        public MergeOperation(OpenApiDocument other, int ordinal)
        {
            _other = other;
            Ordinal = ordinal;
        }
        public int Ordinal { get; }
        public void Apply(OpenApiDocument document)
        {
            if (_other == null) return;
            foreach (var path in _other.Paths)
            {
                if (!document.Paths.ContainsKey(path.Key))
                {
                    document.Paths.Add(path.Key, path.Value);
                    continue;
                }

                throw new NotImplementedException("Handle conflicts");
            }

            foreach (var scheme in _other.Components.Schemas)
            {
                if (!document.Components.Schemas.ContainsKey(scheme.Key))
                {
                    document.Components.Schemas.Add(scheme.Key, scheme.Value);
                    continue;
                }

                throw new NotImplementedException("Handle conflicts");
            }
        }
    }
}
