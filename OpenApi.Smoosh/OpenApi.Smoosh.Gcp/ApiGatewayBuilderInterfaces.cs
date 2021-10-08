using System;
using OpenApi.Smoosh.Common;

namespace OpenApi.Smoosh.Gcp
{
    public interface IApiGatewayFilterStep
    {
        IChooseGcpService ExcludeByPath(params Predicate<string>[] matches);
        IChooseGcpService KeepByPath(params Predicate<string>[] matches);
        IChooseGcpService AdjustPath(Func<string, string> transform);
        IApiGatewayNextStep Build();
    }

    public interface IChooseGcpService
    {
    }

    public interface IApiGatewayNextStep
    {

    }
}
