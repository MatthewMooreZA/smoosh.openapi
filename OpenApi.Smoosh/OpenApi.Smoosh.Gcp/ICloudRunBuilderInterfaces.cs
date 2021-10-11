using System;
using OpenApi.Smoosh.Gcp.Models;

namespace OpenApi.Smoosh.Gcp
{
    public interface ICloudRunFilterRoutesStep : ICloudRunHostStep
    {
        ICloudRunHostStep WithPaths(params Predicate<string>[] filters);
    }
    public interface ICloudRunHostStep
    {
        IProtocolOrSecurityStep WithUrl(string url);
    }

    public interface IProtocolOrSecurityStep : ISecurityStep
    {
        ISecurityStep WithProtocol(Protocols protocol);
    }

    public interface ISecurityStep
    {
        ITimeoutStep WithApiKey();
        ITimeoutStep WithNoAuth();
    }

    public interface ITimeoutStep
    {
        ICloudRunNext WithTimeout(TimeSpan timespan);
    }
    public interface ICloudRunNext
    {

    }
}
