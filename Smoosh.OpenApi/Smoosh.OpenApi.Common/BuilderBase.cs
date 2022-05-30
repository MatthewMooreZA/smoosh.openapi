using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Smoosh.OpenApi.Common.Extensions;
using Smoosh.OpenApi.Common.Models;
using Smoosh.OpenApi.Common.Operations;

namespace Smoosh.OpenApi.Common
{
    public abstract class BuilderBase: IBuilderFilterStep, IBuilderMutateStep, INextStep, IBuilderBuilt
    {
        internal OpenApiDocument Document;

        private readonly Queue<IOperation> _operations = new Queue<IOperation>();

        internal FilterPathsOperation FilterPathsOperation;

        private int _mergeCount = 0;

        internal Dictionary<string, string> RemappedPathReverseLookup = new Dictionary<string, string>();

        public void AddOperation(IOperation operation)
        {
            _operations.Enqueue(operation);
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
                FilterPathsOperation = new FilterPathsOperation(strategy);
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
            var operation = new AdjustPathsOperation(transform);
            AddOperation(operation);

            operation.OnPathAdjustment += (oldPath, newPath, _) =>
            {
                RemappedPathReverseLookup.Add(newPath, oldPath);
            };

            return this;
        }


        public IBuilderBuilt Build()
        {
            while (_operations.Count > 0)
            {
                var operation = _operations.Dequeue();
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

            var merge = new MergeOperation(otherBuilder.Document, _mergeCount++);
            AddOperation(merge);

            foreach (var mapping in otherBuilder.RemappedPathReverseLookup)
            {
                RemappedPathReverseLookup.Add(mapping.Key, mapping.Value);
            }

            return this;
        }

        public OpenApiDocument ToOpenApiDocument()
        {
            return Document;
        }
    }
}
