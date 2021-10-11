using OpenApi.Smoosh.Common;

namespace OpenApi.Smoosh.Gcp
{
    public static class ApiGatewayBuilderExtensions
    {
        public static IApiGatewayFilterStep WithApiGateway(this IBuilderBuilt builder)
        {
            return ApiGatewayBuilder.FromBuilder(builder);
        }

    }
}
