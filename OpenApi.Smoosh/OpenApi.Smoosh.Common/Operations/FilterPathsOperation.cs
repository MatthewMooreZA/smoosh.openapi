using System.Linq;
using Microsoft.OpenApi.Models;
using OpenApi.Smoosh.Common.Models;

namespace OpenApi.Smoosh.Common.Operations
{
    public class FilterPathsOperation : IOperation
    {
        public PathFilterStrategy Strategy { get; }

        public FilterPathsOperation(PathFilterStrategy strategy)
        {
            Strategy = strategy;
        }

        public void Apply(OpenApiDocument document)
        {
            var toRemove =
                document
                    .Paths
                    .Keys
                    .Where(Strategy.Remove)
                    .ToList();

            foreach (var item in toRemove)
            {
                document.Paths.Remove(item);
            }
        }
    }
}
