using Microsoft.OpenApi.Models;

namespace Smoosh.OpenApi.Common.Operations
{
    public interface IOperation
    {
        void Apply(OpenApiDocument document);
    }
}
