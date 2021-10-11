using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Writers;
using OpenApi.Smoosh.Gcp;
using OpenApi.Smoosh.Gcp.Models;
using Xunit;

namespace OpenApi.Smoosh.Tests
{
    public class CloudRun
    {
        [Fact]
        public void MapToCloudRunBasic()
        {
            var next =
                ApiGatewayBuilder
                    .FromOpenApi("./Samples/2.0.uber.json")
                    .MapToCloudRun(config => config
                        .WithUrl("https://is-this-thing-on.com")
                        .WithProtocol(Protocols.Http2)
                        .WithApiKey()
                        .WithTimeout(TimeSpan.FromSeconds(15)));
                
            var build = next.Build();

            Assert.True(build.Components.SecuritySchemes.Any());

            next.ToJson("test.json");
        }
    }
}
