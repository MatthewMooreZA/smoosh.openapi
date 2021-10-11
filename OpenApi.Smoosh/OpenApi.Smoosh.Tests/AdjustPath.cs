using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.OpenApi.Readers;
using OpenApi.Smoosh.Common;
using OpenApi.Smoosh.Common.Operations;
using Xunit;

namespace OpenApi.Smoosh.Tests
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
