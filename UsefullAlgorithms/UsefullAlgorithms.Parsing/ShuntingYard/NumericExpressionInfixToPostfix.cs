using System;
using System.Text.RegularExpressions;

namespace UsefullAlgorithms.Parsing.ExpressionParsing
{
    public class NumericExpressionInfixToPostfix : ShuntingYard<string, string>
    {
        public NumericExpressionInfixToPostfix()
            : base()
        {
            this.operators.Add("+", new PrecedenceAssociativity(0, Associativity.Left));
            this.operators.Add("-", new PrecedenceAssociativity(0, Associativity.Left));
            this.operators.Add("*", new PrecedenceAssociativity(0, Associativity.Left));
            this.operators.Add("/", new PrecedenceAssociativity(5, Associativity.Left));
            this.operators.Add("%", new PrecedenceAssociativity(5, Associativity.Left));
            this.operators.Add("^", new PrecedenceAssociativity(10, Associativity.Right));
        }

        public override string[] Parse(string expression) => InfixToPostfix(expression.Split(' '));
        
        protected override bool IsLeftParenthesis(string token) => token == "(";
        protected override bool IsRightParenthesis(string token) => token == ")";
        protected override bool IsSkippable(string token) => token == " ";
        protected override bool IsComma(string token) => token == ",";
        protected override bool IsWord(string token) => Regex.IsMatch(token, "[a-zA-Z]+");
        protected override bool IsValue(string token) => Regex.IsMatch("[1-9]+", token);

        protected override string GenerateVarArgToken(int argsCount) => string.Format("vararg_{0}", argsCount);
        protected override string GenerateFunctionToken(string oldToken) => oldToken;

    }
}
