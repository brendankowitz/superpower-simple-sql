namespace FhirQL.Expressions
{
    public class TableExpression : Expression
    {
        public string TableName { get; }

        public TableExpression(string tableName)
        {
            TableName = tableName;
        }

        public override string ToString()
        {
            return TableName;
        }
    }
}