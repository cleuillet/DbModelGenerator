using System.Collections.Immutable;
using DbModelGenerator.Util;

namespace DbModelGenerator.Parser.Ast.Constraint
{
    public class PrimaryKeyConstraint : ColumnConstraint
    {
        public PrimaryKeyConstraint(ImmutableSortedSet<string> columns)
        {
            Columns = columns;
        }

        public sealed override ImmutableSortedSet<string> Columns { get; protected set; }

        protected bool Equals(PrimaryKeyConstraint other)
        {
            return Equals(Columns, other.Columns);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PrimaryKeyConstraint) obj);
        }

        public override int GetHashCode()
        {
            return (Columns != null ? Columns.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return ToStringHelper.ToString(this);
        }
    }
}