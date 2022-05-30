using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Smoosh.OpenApi.Common.Extensions;

namespace Smoosh.OpenApi.Gcp.Models
{
    public class GoogleBackendExtension
    {
        private readonly Dictionary<string, IOpenApiAny> _openApiElements = new OpenApiObject();

        private const string Address = "address";
        private const string Protocol = "protocol";
        private const string PathTranslation = "path_translation";
        private const string Timeout = "deadline";

        public void SetUrl(string url)
        {
            _openApiElements.TryRemove(Address);
            _openApiElements.TryRemove(PathTranslation);
            _openApiElements.Add(Address, new OpenApiString(url));
            _openApiElements.Add(PathTranslation, new OpenApiString("APPEND_PATH_TO_ADDRESS"));
        }

        public void SetRemappedUrl(string remappedUrl)
        {
            _openApiElements.TryRemove(Address);
            _openApiElements.TryRemove(PathTranslation);

            _openApiElements.Add(Address, new OpenApiString(remappedUrl));
            _openApiElements.Add(PathTranslation, new OpenApiString("CONSTANT_ADDRESS"));
        }

        public void SetProtocol(Protocols protocol)
        {
            _openApiElements.TryRemove(Protocol);
            _openApiElements.Add(Protocol, new OpenApiString(ProtocolToString(protocol)));
        }

        public void SetTimeout(TimeSpan duration)
        {
            _openApiElements.TryRemove(Timeout);
            _openApiElements.Add(Timeout, new OpenApiDouble(duration.TotalSeconds));
        }

        public OpenApiObject ToOpenApiObject()
        {
            var obj = new OpenApiObject();
            foreach (var element in _openApiElements)
            {
                obj.Add(element.Key, element.Value);
            }

            return obj;
        }

        private string ProtocolToString(Protocols protocol)
        {
            return protocol == Protocols.Http2 ? "h2" : "http/1.1";
        }
    }
}
