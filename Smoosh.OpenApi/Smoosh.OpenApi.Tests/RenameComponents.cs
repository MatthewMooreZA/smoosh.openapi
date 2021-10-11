using System.IO;
using System.Linq;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Smoosh.OpenApi.Common.Extensions;
using Smoosh.OpenApi.Common.Operations;
using Xunit;

namespace Smoosh.OpenApi.Tests
{
    public class RenameComponents
    {
        [Theory]
        [InlineData("./Samples/2.0.petstore-simple.json")]
        [InlineData("./Samples/2.0.uber.json")]
        public void Rename_Should_Apply_Suffix_To_Schemas(string path)
        {
            var doc = new OpenApiStreamReader().Read(File.OpenRead(path), out var diagnostic);

            var rename = new RenameComponentsOperation("test");

            rename.Apply(doc);
            Assert.True(doc.Components.Schemas.Keys.All(x => x.EndsWith("_test")));
        }

        [Theory]
        [InlineData("./Samples/2.0.petstore-simple.json")]
        [InlineData("./Samples/2.0.uber.json")]
        public void Rename_Should_Resolve_Schema_References(string path)
        {
            var doc = new OpenApiStreamReader().Read(File.OpenRead(path), out var diagnostic);

            var rename = new RenameComponentsOperation("test");

            rename.Apply(doc);

            var schemaReferences = doc.ReferencesOf(ReferenceType.Schema).ToList();

            foreach (var schemaRef in schemaReferences)
            {
                Assert.True(doc.Components.Schemas.Keys.Contains(schemaRef.Id));
            }
        }

        [Fact]
        public void Rename_Should_Apply_Suffix_To_Operations()
        {
            var doc = new OpenApiStreamReader().Read(File.OpenRead("./Samples/2.0.petstore-simple.json"), out var diagnostic);

            var rename = new RenameComponentsOperation("test");

            rename.Apply(doc);
            Assert.True(doc.Paths.Values.SelectMany(p => p.Operations.Values).All(x => x.OperationId.EndsWith("_test")));
        }
    }
}
