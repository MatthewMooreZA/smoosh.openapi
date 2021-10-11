using System;
using OpenApi.Smoosh.Gcp.Models;
using OpenApi.Smoosh.Gcp.Operations;

namespace OpenApi.Smoosh.Gcp
{
    public class CloudRunBuilder : ICloudRunFilterRoutesStep, IProtocolOrSecurityStep, ISecurityStep, ITimeoutStep, ICloudRunNext
    {
        private readonly CloudRunOperation _operation;
        public CloudRunBuilder(CloudRunOperation operation)
        {
            _operation = operation;
        }

        public IProtocolOrSecurityStep WithUrl(string url)
        {
            _operation.CloudRunUrl = url;
            return this;
        }

        public ICloudRunHostStep WithPaths(params Predicate<string>[] filters)
        {
            _operation.PathFilter.AddRange(filters);
            return this;
        }

        public ISecurityStep WithProtocol(Protocols protocol)
        {
            _operation.Protocol = protocol;
            return this;
        }


        public ITimeoutStep WithApiKey()
        {
            _operation.Security = Security.ApiKey;
            return this;
        }

        public ITimeoutStep WithNoAuth()
        {
            _operation.Security = Security.None;
            return this;
        }

        public ICloudRunNext WithTimeout(TimeSpan timespan)
        {
            _operation.Timeout = timespan;
            return this;
        }
    }
}
