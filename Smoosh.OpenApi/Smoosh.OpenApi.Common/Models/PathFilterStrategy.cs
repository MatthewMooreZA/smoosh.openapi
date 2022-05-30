using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.SmartEnum;

namespace Smoosh.OpenApi.Common.Models
{
    public abstract class PathFilterStrategy : SmartEnum<PathFilterStrategy>
    {
        public static PathFilterStrategy None => new NoneStrategy();
        public static PathFilterStrategy Keep => new KeepStrategy();
        public static PathFilterStrategy Exclude => new ExcludeStrategy();

        private readonly List<Predicate<string>> _predicates = new List<Predicate<string>>();
        protected PathFilterStrategy(string name, int value) : base(name, value)
        {
        }

        public abstract bool Remove(string route);

        public void AddPredicates(Predicate<string>[] predicates)
        {
            _predicates.AddRange(predicates);
        }
        private sealed class NoneStrategy : PathFilterStrategy
        {
            public NoneStrategy() : base("None", 0) {}
            public override bool Remove(string route) => false;
        }

        private sealed class KeepStrategy : PathFilterStrategy
        {
            public KeepStrategy() : base("Keep", 1) {}
            public override bool Remove(string route) => _predicates.All(p => !p.Invoke(route));
        }

        private sealed class ExcludeStrategy : PathFilterStrategy
        {
            public ExcludeStrategy() : base("Exclude", 2) { }
            public override bool Remove(string route) => _predicates.Any(p => p.Invoke(route));
        }
    }
}
