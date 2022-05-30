using System.IO;
using Microsoft.OpenApi.Readers;
using Smoosh.OpenApi.Common.Operations;
using Xunit;

namespace Smoosh.OpenApi.Tests
{
    public class MergeOperations
    {
        [Fact]
        public void Merge_With_No_Conflicts()
        {
            var petDoc = new OpenApiStreamReader().Read(File.OpenRead("./Samples/2.0.petstore-simple.json"), out _);
            var uberDoc = new OpenApiStreamReader().Read(File.OpenRead("./Samples/2.0.uber.json"), out _);

            int expectedPaths = petDoc.Paths.Count + uberDoc.Paths.Count;

            int expectedSchemes = petDoc.Components.Schemas.Count + uberDoc.Components.Schemas.Count;

            var mergeOperation = new MergeOperation(uberDoc, 1);

            mergeOperation.Apply(petDoc); // merge uber onto pets

            Assert.Equal(expectedPaths, petDoc.Paths.Count);
            Assert.Equal(expectedSchemes, petDoc.Components.Schemas.Count);
        }
    }
}
