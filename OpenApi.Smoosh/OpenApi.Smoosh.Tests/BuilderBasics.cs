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
            var builder = new Builder();

            builder.FromOpenApi(fs);

            Assert.NotEmpty(builder.Document.Paths);
        }

        public void FromOpenApiFluent()
        {
            Builder.Init()
                .FromOpenApi("./Samples/2.0.petstore-simple.json")
                .KeepByPath
                (
                    f => f.Contains("id")
                );
        }


        [Fact]
        public void AdjustPath_Prefix()
        {
            using var fs = File.OpenRead("./Samples/2.0.petstore-simple.json");
            var builder = new Builder();

            builder.FromOpenApi(fs);

            builder.AdjustPath(p => "/v1" + p);
            builder.Build();
            Assert.True(builder.Document.Paths.All(x => x.Key.StartsWith("/v1/")));
        }
    }
}
