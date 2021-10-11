using System.IO;
using System.Linq;
using OpenApi.Smoosh.Common;
using Xunit;

namespace OpenApi.Smoosh.Tests
{
    public class BuilderBasics
    {
        [Fact]
        public void FromOpenApi()
        {
            using var fs = File.OpenRead("./Samples/2.0.petstore-simple.json");
            var builder = (Builder)Builder.FromOpenApi(fs);

            Assert.NotEmpty(builder.Document.Paths);
        }

        [Fact]
        public void FromOpenApi_Fluent()
        {
            var api = Builder
                .FromOpenApi("./Samples/2.0.petstore-simple.json")
                .Build();

            Assert.NotEmpty(((Builder)api).Document.Paths);
        }


        [Fact]
        public void AdjustPath_Prefix()
        {
            using var fs = File.OpenRead("./Samples/2.0.petstore-simple.json");
            var builder = (Builder)Builder.FromOpenApi(fs);

            builder.AdjustPath(p => "/v1" + p);
            builder.Build();
            Assert.True(builder.Document.Paths.All(x => x.Key.StartsWith("/v1/")));
        }

        [Fact]
        public void AdjustPath_Should_ContainReverseMap()
        {
            using var fs = File.OpenRead("./Samples/2.0.petstore-simple.json");
            var builder = (Builder)Builder.FromOpenApi(fs);

            builder.AdjustPath(p => "/v1" + p);
            builder.Build();
            Assert.NotEmpty(builder.RemappedPathReverseLookup);
        }

        [Fact]
        public void AdjustPath_Prefix_Fluent()
        {
            var api = Builder
                .FromOpenApi("./Samples/2.0.petstore-simple.json")
                .AdjustPath(p => "/v1" + p)
                .Build();

            Assert.True(((Builder)api).Document.Paths.All(x => x.Key.StartsWith("/v1/")));
        }
    }
}
