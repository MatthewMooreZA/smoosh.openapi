using System;
using System.IO;
using System.Linq;
using Microsoft.OpenApi.Readers;
using OpenApi.Smoosh.Common;
using OpenApi.Smoosh.Common.Models;
using OpenApi.Smoosh.Common.Operations;
using Xunit;

namespace OpenApi.Smoosh.Tests
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

            var mergeOperation = new MergeOperation(uberDoc, 0);

            mergeOperation.Apply(petDoc); // merge uber onto pets

            Assert.Equal(expectedPaths, petDoc.Paths.Count);
            Assert.Equal(expectedSchemes, petDoc.Components.Schemas.Count);
        }
    }
}
