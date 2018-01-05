namespace FhirQL.Expressions
{
    public class EqualityExpression : Expression
    {
        public string Operator { get; }
        public Expression[] Operands { get; }

        public EqualityExpression(string @operator, params Expression[] operands)
        {
            Operator = @operator;
            Operands = operands;
        }
    }
}