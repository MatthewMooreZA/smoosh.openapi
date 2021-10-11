using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using OpenApi.Smoosh.Common.Extensions;
using OpenApi.Smoosh.Common.Models;
using OpenApi.Smoosh.Common.Operations;

namespace OpenApi.Smoosh.Common
{
    public abstract class BuilderBase: IBuilderFilterStep, IBuilderMutateStep, INextStep, IBuilderBuilt
    {
        internal OpenApiDocument Document;

        private readonly List<IOperation> _operations = new List<IOperation>();
        private int _operationCount = 0;

        internal FilterPathsOperation FilterPathsOperation;

        public void AddOperation(IOperation operation)
        {
            _operations.Add(operation);
        }

        public IBuilderMutateStep ExcludeByPath(params Predicate<string>[] matches)
        {
            SetPathFilterStrategy(PathFilterStrategy.Exclude);
            FilterPathsOperation.Strategy.AddPredicates(matches);
            return this;
        }

        private void SetPathFilterStrategy(PathFilterStrategy strategy)
        {
            if (FilterPathsOperation == null)
            {
                FilterPathsOperation = new FilterPathsOperation(strategy, _operationCount++);
                AddOperation(FilterPathsOperation);
            }
            else if (FilterPathsOperation.Strategy != strategy)
            {
                throw new InvalidOperationException("Route Filter strategies are mutually exclusive.");
            }
        }

        public IBuilderMutateStep KeepByPath(params Predicate<string>[] matches)
        {
            SetPathFilterStrategy(PathFilterStrategy.Keep);
            FilterPathsOperation.Strategy.AddPredicates(matches);
            return this;
        }

        public INextStep AdjustPath(Func<string, string> transform)
        {
            AddOperation(new AdjustPathsOperation(transform, _operationCount++));
            return this;
        }

        public IBuilderBuilt Build()
        {
            foreach (var operation in _operations.OrderBy(op => op.Ordinal))
            {
                operation.Apply(Document);
            }

            var version = GetType().Assembly.GetName().Version.ToString();
            Document.Extensions.TryRemove("x-generator");
            Document.Extensions.Add("x-generator", new OpenApiString($"OpenApi Smoosh - {version}"));

            return this;
        }

        public IBuilderBuilt Merge(IBuilderBuilt other)
        {
            if (!(other is Builder otherBuilder)) return this;

            var merge = new MergeOperation(otherBuilder.Document, _operationCount++);
            AddOperation(merge);

            merge.Apply(this.Document);

            return this;
        }

        public OpenApiDocument ToOpenApiDocument()
        {
            return Document;
        }
    }
}
