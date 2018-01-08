using FhirQL.Clauses;
using FhirQL.Expressions;
using Superpower;
using Superpower.Parsers;

namespace FhirQL
{
    public static class SqlParser
    {
        public static TokenListParser<SqlToken, Expression> ColumnName =
            Token.EqualTo(SqlToken.Keyword)
                .Select(n => (Expression)new ColumnExpression(n.ToStringValue()));

        public static TokenListParser<SqlToken, Expression> TableName =
            Token.EqualToValue(SqlToken.Keyword, "Patient")
                .Or(Token.EqualToValue(SqlToken.Keyword, "Observation"))
            .Select(n => (Expression)new TableExpression(n.ToStringValue()));

        public static TokenListParser<SqlToken, Expression> Table =
            Token.EqualTo(SqlToken.Keyword)
                .Select(n => (Expression)new ColumnExpression(n.ToStringValue()));

        public static TokenListParser<SqlToken, Expression> Call = ColumnName;

        public static TokenListParser<SqlToken, Expression> Expression = Call;

        public static TokenListParser<SqlToken, Statement> Statement =
            from keyword in Token.EqualToValue(SqlToken.Keyword, "select")
            from columns in Expression.ManyDelimitedBy(Token.EqualTo(SqlToken.Comma))
            from @from in Token.EqualToValue(SqlToken.Keyword, "from")
            from tables in TableName.AtLeastOnce()
            select new Statement(new SelectClause(columns), new FromClause(tables), new WhereClause(new Expression[0]));
    }
}