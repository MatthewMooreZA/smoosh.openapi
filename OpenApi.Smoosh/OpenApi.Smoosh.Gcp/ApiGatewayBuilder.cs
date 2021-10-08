using System;
using System.IO;
using OpenApi.Smoosh.Common;

namespace OpenApi.Smoosh.Gcp
{
    public class ApiGatewayBuilder : IApiGatewayFilterStep, IChooseGcpService, IApiGatewayNextStep
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

        public static IBuilderFilterStep FromOpenApi(string file)
        {
            return Builder.FromOpenApi(File.OpenRead(file));
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

        public IApiGatewayNextStep Build()
        {
            throw new NotImplementedException();
        }
    }
}
