using Ardalis.SmartEnum;
using OpenApi.Smoosh.Common.Exceptions;

namespace OpenApi.Smoosh.Common.Models
{
    public abstract class MergeConflictStrategy : SmartEnum<MergeConflictStrategy>
    {
        public static MergeConflictStrategy Throw = new ThrowStrategy();
        public static MergeConflictStrategy RetainOld = new RetainOldStrategy();
        public static MergeConflictStrategy RetainNew = new RetainNewStrategy();

        public abstract int ResolveConflict(string old, string @new);

        protected MergeConflictStrategy(string name, int value) : base(name, value)
        {
        }

        private sealed class ThrowStrategy : MergeConflictStrategy
        {
            public ThrowStrategy() : base("Throw", 0){}

            public override int ResolveConflict(string old, string @new)
            {
                throw new MergeConflictException();
            }
        }

        private sealed class RetainOldStrategy : MergeConflictStrategy
        {
            public RetainOldStrategy() : base("RetainOld", 0) { }

            public override int ResolveConflict(string old, string @new) => -1;
        }

        private sealed class RetainNewStrategy : MergeConflictStrategy
        {
            public RetainNewStrategy() : base("RetainNew", 0) { }

            public override int ResolveConflict(string old, string @new) => 1;
        }
    }
}
