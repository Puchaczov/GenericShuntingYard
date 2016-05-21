using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ExpressionParsing
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
    }
}
