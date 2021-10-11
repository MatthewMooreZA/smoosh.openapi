using System;
using System.IO;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using OpenApi.Smoosh.Common;
using OpenApi.Smoosh.Gcp.Operations;

namespace OpenApi.Smoosh.Gcp
{
    public class ApiGatewayBuilder : IApiGatewayFilterStep, IChooseGcpService
    {
        internal Builder Builder;
        protected internal ApiGatewayBuilder(){}

        public static IApiGatewayFilterStep FromOpenApi(Stream stream)
        {
            return new ApiGatewayBuilder
            {
                Builder = (Builder)Builder.FromOpenApi(stream)
            };
        }

        public static IApiGatewayFilterStep FromBuilder(IBuilderBuilt builder)
        {
            if (!(builder is Builder genericBuilder))
            {
                throw new ArgumentException();
            }

            return new ApiGatewayBuilder
            {
                Builder = genericBuilder
            };
        }

        public static IApiGatewayFilterStep FromOpenApi(string file)
        {
            return FromOpenApi(File.OpenRead(file));
        }

        public IChooseGcpService ExcludeByPath(params Predicate<string>[] matches)
        {
            Builder.ExcludeByPath(matches);
            return this;
        }

        public IChooseGcpService KeepByPath(params Predicate<string>[] matches)
        {
            Builder.KeepByPath(matches);
            return this;
        }

        public IChooseGcpService AdjustPath(Func<string, string> transform)
        {
            Builder.AdjustPath(transform);
            return this;
        }


        public IChooseGcpService MapToCloudRun(Func<ICloudRunFilterRoutesStep, ICloudRunNext> config)
        {
            var operation = new CloudRunOperation();
            Builder.AddOperation(operation);

            var cloudRunBuilder = new CloudRunBuilder(operation);

            config.Invoke(cloudRunBuilder);

            return this;
        }

        private IBuilderBuilt _built = null;

        public OpenApiDocument Build()
        {
            if (_built == null)
            {
                _built = Builder.Build();
            }
            return _built.ToOpenApiDocument();
        }

        public void ToJson(string path)
        {
            var doc = Build();
            var json = doc.Serialize(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);
            File.WriteAllText(path, json);
        }

        public void ToYaml(string path)
        {
            var doc = Build();
            var yaml = doc.Serialize(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Yaml);
            File.WriteAllText(path, yaml);
        }
    }
}
