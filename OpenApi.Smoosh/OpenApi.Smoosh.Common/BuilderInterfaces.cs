using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.OpenApi.Models;

namespace OpenApi.Smoosh.Common
{
    public interface IBuilderFilterStep
    {
        IBuilderMutateStep ExcludeByPath(params Predicate<string>[] matches);
        IBuilderMutateStep KeepByPath(params Predicate<string>[] matches);
        INextStep AdjustPath(Func<string, string> transform);
        IBuilderBuilt Build();
    }

    public interface IBuilderMutateStep
    {
        INextStep AdjustPath(Func<string, string> transform);
        IBuilderBuilt Build();
    }

    public interface INextStep // TBD
    {
        IBuilderBuilt Build();
    }

    public interface IBuilderBuilt
    {
        IBuilderBuilt Merge(IBuilderBuilt other);
        OpenApiDocument ToOpenApiDocument();
    }
}
