using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FhirQL.Clauses;
using FhirQL.Expressions;

namespace FhirQL
{
    public class FhirSqlFormatter
    {
        public static string ToSql(Statement statement)
        {
            var builder = new StringBuilder();

            Visit(builder, statement.Select);
            var recordFilter = Visit(builder, statement.From).ToArray();
            Visit(builder, statement.Where, recordFilter);

            return builder.ToString();
        }

        private static void Visit(StringBuilder builder, WhereClause statementWhere, IEnumerable<Expression> additionalWhere)
        {
            builder.Append(" WHERE ");
            foreach (var statement in statementWhere.Expressions.Concat(additionalWhere))
            {
                VisitWhere(builder, statement);
            }
        }

        public static void VisitWhere(StringBuilder builder, Expression expression)
        {
            if (expression is CallExpression call)
            {
                VisitWhere(builder, call.Operands.First());
                builder.AppendFormat("{0} ", call.Operator);
                VisitWhere(builder, call.Operands.Last());
                builder.Append(" ");
            }
            else if (expression is EqualityExpression equality)
            {
                VisitWhere(builder, equality.Operands.First());
                builder.AppendFormat("{0} ", equality.Operator);
                VisitWhere(builder, equality.Operands.Last());
                builder.Append(" ");
            }
            else if (expression is ColumnExpression column)
            {
                builder.AppendFormat("{0}", column.ColumnName);
            }
            else if (expression is ConstantStringExpression constString)
            {
                builder.AppendFormat("'{0}'", constString.Value.Replace("'", "''"));
            }
            else if (expression is ConstantExpression constInt)
            {
                builder.AppendFormat("{0}", constInt.Value);
            }
        }

        private static IEnumerable<Expression> Visit(StringBuilder builder, FromClause statementFrom)
        {
            builder.Append(" FROM ");
            builder.Append(string.Join(" ", statementFrom.FromExpressions.OfType<TableExpression>().Select(Visit)));

            foreach (var table in statementFrom.FromExpressions.OfType<TableExpression>())
            {
                yield return
                    new CallExpression("AND",
                    new EqualityExpression("=", new ColumnExpression("ResourceTypeName"), new ConstantStringExpression(table.TableName)),
                    new EqualityExpression("=", new ColumnExpression("IsHistory"), new ConstantExpression(0)));
            }
        }

        private static string Visit(TableExpression tableExpression)
        {
            return "Resources";
        }

        private static void Visit(StringBuilder builder, SelectClause selectClause)
        {
            builder.Append("SELECT ");
            builder.Append(string.Join(", ", selectClause.Columns.OfType<ColumnExpression>().Select(Visit)));
        }

        private static string Visit(ColumnExpression column)
        {
            return string.Format("JSON_VALUE(RawResourceData, '$.{0}') AS [{1}]", column.ColumnName, ToSlug(column.ColumnName));
        }

        private static string ToSlug(string phrase)
        {
            var s = phrase;
            s = Regex.Replace(s, @"[^a-zA-Z0-9\s-]", "");                      // remove invalid characters
            s = Regex.Replace(s, @"\s+", " ").Trim();                       // single space
            s = Regex.Replace(s, @"\s", "-");                               // insert hyphens
            return s.ToLower();
        }
    }
}