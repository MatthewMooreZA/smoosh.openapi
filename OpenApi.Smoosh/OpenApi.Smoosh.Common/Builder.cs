using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using OpenApi.Smoosh.Common.Extensions;
using OpenApi.Smoosh.Common.Models;
using OpenApi.Smoosh.Common.Operations;

namespace OpenApi.Smoosh.Common
{
    public class Builder : IBuilderInitStep, IBuilderFilterStep, IBuilderMutateStep, INextStep, IBuilderBuilt
    {
        internal OpenApiDocument Document;

        private readonly List<IOperation> _operations = new List<IOperation>();
        private int _operationCount = 0;

        private FilterPathsOperation _filterPathsOperation;

        protected internal Builder(){}

        public static IBuilderInitStep Init()
        {
            return new Builder();
        }

        public IBuilderFilterStep FromOpenApi(Stream stream)
        {
            Document = new OpenApiStreamReader().Read(stream, out var diagnostic);
            return this;
        }

        public IBuilderFilterStep FromOpenApi(string file)
        {
            return this.FromOpenApi(File.OpenRead(file));
        }

        public IBuilderMutateStep ExcludeByPath(params Predicate<string>[] matches)
        {
            SetPathFilterStrategy(PathFilterStrategy.Exclude);
            _filterPathsOperation.Strategy.AddPredicates(matches);
            return this;
        }

        private void SetPathFilterStrategy(PathFilterStrategy strategy)
        {
            if (_filterPathsOperation == null)
            {
                _filterPathsOperation = new FilterPathsOperation(strategy, _operationCount++);
                _operations.Add(_filterPathsOperation);
            }
            else if (_filterPathsOperation.Strategy != strategy)
            {
                throw new InvalidOperationException("Route Filter strategies are mutually exclusive.");
            }
        }

        public IBuilderMutateStep KeepByPath(params Predicate<string>[] matches)
        {
            SetPathFilterStrategy(PathFilterStrategy.Keep);
            _filterPathsOperation.Strategy.AddPredicates(matches);
            return this;
        }

        public INextStep AdjustPath(Func<string, string> transform)
        {
            _operations.Add(new AdjustPathsOperation(transform, _operationCount++));
            return this;
        }

        public IBuilderBuilt Build()
        {
            foreach (var operation in _operations.OrderBy(op => op.Ordinal))
            {
                operation.Apply(Document);
            } 

            return this;
        }

        public IBuilderBuilt Merge(IBuilderBuilt other)
        {
            var otherBuilder = other as Builder;
            if (otherBuilder == null) return this;

            var merge = new MergeOperation(otherBuilder.Document, _operationCount++);
            _operations.Add(merge);

            merge.Apply(this.Document);

            return this;
        }
    }
}
