using System.Collections.Generic;
using Ardalis.SmartEnum;
using Microsoft.OpenApi.Models;

namespace Smoosh.OpenApi.Gcp.Models
{
    public abstract class Security : SmartEnum<Security>
    {
        public static Security None => new NoSecurity();
        public static Security ApiKey => new ApiKeySecurity();

        public abstract void Apply(OpenApiDocument document);
        public abstract void Apply(OpenApiDocument document, OpenApiOperation operation);

        protected Security(string name, int value) : base(name, value)
        {
        }

        protected class NoSecurity : Security
        {
            public NoSecurity() : base("None", 0)
            {
            }

            public override void Apply(OpenApiDocument document)
            {
                // noop
            }

            public override void Apply(OpenApiDocument document, OpenApiOperation operation)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement()
                };
            }
        }

        protected class ApiKeySecurity : Security
        {
            private const string _apiKey = "api_key";

            public ApiKeySecurity() : base("ApiKey", 1)
            {
            }

            public override void Apply(OpenApiDocument document)
            {
                var scheme = UpsertSecurityDefinition(document);
                document.SecurityRequirements = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            scheme, new List<string>()
                        }
                    }
                };
            }

            public override void Apply(OpenApiDocument document, OpenApiOperation operation)
            {
                // noop
            }

            private OpenApiSecurityScheme UpsertSecurityDefinition(OpenApiDocument document)
            {
                // document.Components.SecuritySchemes, operation.Value.Security
                if (document.Components.SecuritySchemes.ContainsKey(_apiKey))
                {
                    return document.Components.SecuritySchemes[_apiKey];
                }

                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "API key needed to access the endpoints",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "X-Api-Key",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = _apiKey
                    },
                    UnresolvedReference = false,
                };

                document.Components.SecuritySchemes.Add(_apiKey, securityScheme);

                return securityScheme;
            }
        }
    }
}
