using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.OpenApi.Readers;

namespace OpenApi.Smoosh.Common
{
    public class Builder : BuilderBase
    {
        protected internal Builder(){}

        public static IBuilderFilterStep FromOpenApi(Stream stream)
        {
            return new Builder
            {
                Document = new OpenApiStreamReader().Read(stream, out var diagnostic)
            };
        }

        public static IBuilderFilterStep FromOpenApi(string file)
        {
            return Builder.FromOpenApi(File.OpenRead(file));
        }

        public IReadOnlyDictionary<string, string> GetRemappedPathReverseLookup()
        {
            return new ReadOnlyDictionary<string, string>(RemappedPathReverseLookup);
        }
    }
}
