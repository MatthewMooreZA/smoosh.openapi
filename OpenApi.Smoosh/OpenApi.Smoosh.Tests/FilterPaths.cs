using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.OpenApi.Readers;
using OpenApi.Smoosh.Common.Models;
using OpenApi.Smoosh.Common.Operations;
using Xunit;

namespace OpenApi.Smoosh.Tests
{
    public class FilterPaths
    {
        [Fact]
        public void ExcludeFilter()
        {
            var doc = new OpenApiStreamReader().Read(File.OpenRead("./Samples/2.0.uber.json"), out var diagnostic);

            var countBefore = doc.Paths.Count;

            var filter = new FilterPathsOperation(PathFilterStrategy.Exclude);
            var toFilter = doc.Paths.First().Key;
            filter.Strategy.AddPredicates(new Predicate<string>[]
            {
                path => path.Equals(toFilter)
            });

            filter.Apply(doc);

            var countAfter = doc.Paths.Count;
            Assert.Equal(countBefore - 1, countAfter);
        }

        [Fact]
        public void RetainFilter()
        {
            var doc = new OpenApiStreamReader().Read(File.OpenRead("./Samples/2.0.uber.json"), out var diagnostic);

            var countBefore = doc.Paths.Count;

            var filter = new FilterPathsOperation(PathFilterStrategy.Keep);
            var toFilter = doc.Paths.First().Key;
            filter.Strategy.AddPredicates(new Predicate<string>[]
            {
                path => path.Equals(toFilter)
            });

            filter.Apply(doc);

            var countAfter = doc.Paths.Count;
            Assert.Equal(1, countAfter);
        }
    }
}
