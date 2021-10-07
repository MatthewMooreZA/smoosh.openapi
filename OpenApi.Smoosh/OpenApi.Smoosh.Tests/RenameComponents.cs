using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using OpenApi.Smoosh.Common.Extensions;
using OpenApi.Smoosh.Common.Operations;
using SharpYaml.Events;
using Xunit;

namespace OpenApi.Smoosh.Tests
{
    public class RenameComponents
    {
        [Theory]
        [InlineData("./Samples/2.0.petstore-simple.json")]
        [InlineData("./Samples/2.0.uber.json")]
        public void Rename_Should_Apply_Suffix_To_Schemas(string path)
        {
            var doc = new OpenApiStreamReader().Read(File.OpenRead(path), out var diagnostic);

            var rename = new RenameComponentsOperation("test", 0);

            rename.Apply(doc);
            Assert.True(doc.Components.Schemas.Keys.All(x => x.EndsWith("_test")));
        }

        [Theory]
        [InlineData("./Samples/2.0.petstore-simple.json")]
        [InlineData("./Samples/2.0.uber.json")]
        public void Rename_Should_Resolve_Schema_References(string path)
        {
            var doc = new OpenApiStreamReader().Read(File.OpenRead(path), out var diagnostic);

            var rename = new RenameComponentsOperation("test", 0);

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

            var rename = new RenameComponentsOperation("test", 0);

            rename.Apply(doc);
            Assert.True(doc.Paths.Values.SelectMany(p => p.Operations.Values).All(x => x.OperationId.EndsWith("_test")));
        }
    }
}
