using System.IO;
using Microsoft.OpenApi.Readers;
using Smoosh.OpenApi.Common.Operations;
using Xunit;

namespace Smoosh.OpenApi.Tests
{
    public class AdjustPath
    {
        [Fact]
        public void AdjustPath_ShouldInvokeAction_If_Set()
        {
            var doc = new OpenApiStreamReader().Read(File.OpenRead("./Samples/2.0.petstore-simple.json"), out var diagnostic);

            var adjust = new AdjustPathsOperation(p => "/v1" + p);

            int pathAdjustmentCount = 0;

            adjust.OnPathAdjustment += (old, @new, doc) => pathAdjustmentCount++;

            adjust.Apply(doc);

            Assert.Equal(doc.Paths.Count, pathAdjustmentCount);
        }
    }
}
