namespace FhirQL.Expressions
{
    public class ConstantStringExpression : Expression
    {
        public string Value { get; }

        public ConstantStringExpression(string @value)
        {
            Value = value;
        }
    }
}