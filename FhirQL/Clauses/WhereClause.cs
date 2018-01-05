using FhirQL.Expressions;

namespace FhirQL.Clauses
{
    public class WhereClause
    {
        public Expression[] Expressions { get; }

        public WhereClause(Expression[] expressions)
        {
            Expressions = expressions;
        }
    }
}