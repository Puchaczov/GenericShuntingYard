using System;
using System.Text.RegularExpressions;
using UsefullAlgorithms.Parsing.ExpressionParsing;

namespace UsefullAlgorightms.Parsing.ExpressionParsing
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

        protected override bool IsComma(string token) => token == ",";

        protected override bool IsWord(string token) => Regex.IsMatch(token, "[a-zA-Z]+");

        protected override string GenerateVarArgToken(int argsCount) => string.Format("vararg_{0}", argsCount);
    }
}
