using System;
using System.IO;
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
            using (var sw = new StreamWriter(path, false))
            {
                doc.SerializeAsV2(new OpenApiJsonWriter(sw));
            }
        }

        public void ToYaml(string path)
        {
            var doc = Build();
            using (var sw = new StreamWriter(path, false))
            {
                doc.SerializeAsV2(new OpenApiYamlWriter(sw));
            }
        }
    }
}
