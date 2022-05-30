# Smoosh OpenApi

OpenApi transformers utilities

# Generic builder

Basic operations for working with OpenApi documents.

*TODO: expand list with examples*

# Google Cloud

## Cloud Run Examples

### Example 1

Basic mapping to backend with API key.

```csharp
ApiGatewayBuilder
    .FromOpenApi("./Samples/2.0.uber.json")
    .MapToCloudRun(config => config
        .WithUrl("https://is-this-thing-on.com")
        .WithProtocol(Protocols.Http2)
        .WithApiKey()
    ).ToJson("api-gateway.json");
```

### Example 2

Later configurations will override earlier ones.  This allows you to start with general behaviour and then adjust it by exception.

Here we remove the API key constraint and extend the timeout for all paths that start with '/estimates'.


```csharp
var url = "https://uber-pets.a.run.app";
ApiGatewayBuilder
    .FromOpenApi("./Samples/2.0.uber.json")
    .MapToCloudRun(config => config
        .WithUrl(url)
        .WithProtocol(Protocols.Http2)
        .WithApiKey()
        .WithTimeout(TimeSpan.FromSeconds(15)))
    .MapToCloudRun(config => config
        .WithPaths(p => p.StartsWith("/estimates"))
        .WithUrl(url)
        .WithProtocol(Protocols.Http2)
        .WithNoAuth()
        .WithTimeout(TimeSpan.FromSeconds(30))
    ).ToYaml("api-gateway.yaml");
```

### Example 3

The abilities of the general ```Builder``` class can be combined with the ```ApiGatewayBuilder``` to do some more interesting things.

```csharp
Builder
    .FromOpenApi("./Samples/2.0.uber.json")
    .Build().Merge(
        Builder
        .FromOpenApi("./Samples/2.0.petstore-simple.json")
        .Build())
    .WithApiGateway()
    .MapToCloudRun(config => config
        .WithUrl("https://uber-pets.a.run.app")
        .WithProtocol(Protocols.Http2)
        .WithApiKey());
```
