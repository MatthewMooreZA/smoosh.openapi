using Microsoft.OpenApi.Models;

namespace OpenApi.Smoosh.Common.Operations
{
    public interface IOperation
    {
        int Ordinal { get; }
        void Apply(OpenApiDocument document);
    }
}
