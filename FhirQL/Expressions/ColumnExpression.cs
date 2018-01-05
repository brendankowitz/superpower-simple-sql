namespace FhirQL.Expressions
{
    public class ColumnExpression : Expression
    {
        public string ColumnName { get; }

        public ColumnExpression(string columnName)
        {
            ColumnName = columnName;
        }

        public override string ToString()
        {
            return ColumnName;
        }
    }
}