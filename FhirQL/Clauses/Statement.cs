namespace FhirQL.Clauses
{
    public class Statement
    {
        public SelectClause Select { get; }
        public FromClause From { get; }
        public WhereClause Where { get;  set; }

        public Statement(SelectClause select, FromClause from, WhereClause where)
        {
            Select = @select;
            From = @from;
            Where = @where;
        }
    }
}