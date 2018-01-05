using FhirQL.Expressions;

namespace FhirQL.Clauses
{
    public class FromClause
    {
        public Expression[] FromExpressions { get; }

        public FromClause(Expression[] fromExpressions)
        {
            FromExpressions = fromExpressions;
        }
    }
}