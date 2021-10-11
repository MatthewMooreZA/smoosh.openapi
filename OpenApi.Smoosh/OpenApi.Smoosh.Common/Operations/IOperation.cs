using Microsoft.OpenApi.Models;

namespace OpenApi.Smoosh.Common.Operations
{
    public interface IOperation
    {
        void Apply(OpenApiDocument document);
    }
}
