using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenApi.Smoosh.Common
{
    public interface IBuilderInitStep
    {
        IBuilderFilterStep FromOpenApi(Stream stream);
        IBuilderFilterStep FromOpenApi(string file);
    }

    public interface IBuilderFilterStep
    {
        IBuilderMutateStep ExcludeByPath(params Predicate<string>[] matches);
        IBuilderMutateStep KeepByPath(params Predicate<string>[] matches);
        IBuilderBuilt Build();
    }

    public interface IBuilderMutateStep
    {
        INextStep AdjustPath(Func<string, string> transform);
        IBuilderBuilt Build();
    }

    public interface INextStep
    {

    }

    public interface IBuilderBuilt
    {
        IBuilderBuilt Merge(IBuilderBuilt other);
    }
}
