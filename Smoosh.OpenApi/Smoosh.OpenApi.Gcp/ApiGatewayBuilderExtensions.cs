using Smoosh.OpenApi.Common;

namespace Smoosh.OpenApi.Gcp
{
    public static class ApiGatewayBuilderExtensions
    {
        public static IApiGatewayFilterStep WithApiGateway(this IBuilderBuilt builder)
        {
            return ApiGatewayBuilder.FromBuilder(builder);
        }

    }
}
