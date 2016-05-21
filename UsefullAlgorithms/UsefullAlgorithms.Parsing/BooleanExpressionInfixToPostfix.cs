using UsefullAlgorithms.ExpressionParsing;

namespace UsefullAlgorightms.ExpressionParsing
{
    public class BooleanExpressionInfixToPostfix : ShuntingYard<string, string>
    {
        public BooleanExpressionInfixToPostfix()
            : base()
        {
            this.operators.Add("or", new PrecedenceAssociativity(0, Associativity.Left));
            this.operators.Add("and", new PrecedenceAssociativity(5, Associativity.Left));
            this.operators.Add("not", new PrecedenceAssociativity(10, Associativity.Right));
        }

        public override string[] Parse(string expression) => InfixToPostfix(expression.Split(' '));

        protected override bool IsLeftParenthesis(string token) => token == "(";

        protected override bool IsRightParenthesis(string token) => token == ")";

        protected override bool IsSkippable(string token) => token == " ";
    }
}
