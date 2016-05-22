using System;
using System.Collections;
using System.Collections.Generic;

namespace UsefullAlgorithms.Parsing.ExpressionParsing
{
    public static class StackHelper
    {
        public static bool IsEmpty<T>(this Stack<T> stack) => stack.Count == 0;
    }


    /// <summary>
    /// Implementation based on https://en.wikipedia.org/wiki/Shunting-yard_algorithm
    /// Idea how to handle function call based on this whole topic: http://stackoverflow.com/questions/29348246/how-to-count-number-of-arguments-of-a-method-while-converting-infix-expression-t
    /// </summary>
    /// <typeparam name="TExpression"></typeparam>
    /// <typeparam name="TToken"></typeparam>
    public abstract class ShuntingYard<TExpression, TToken>
        where TExpression : IEnumerable
        where TToken : IEquatable<TToken>
    {

        private class FunctionArgs
        {
            public int ArgsCount { get; set; }
            public TToken Name { get; set; }
        }

        protected readonly Dictionary<TToken, PrecedenceAssociativity> operators;

        public ShuntingYard()
        {
            operators = new Dictionary<TToken, PrecedenceAssociativity>();
        }

        public ShuntingYard(params KeyValuePair<TToken, PrecedenceAssociativity>[] rules)
        {
            foreach(var rule in rules)
            {
                this.operators.Add(rule.Key, rule.Value);
            }
        }

        public abstract TToken[] Parse(TExpression expression);

        protected TToken[] InfixToPostfix(IEnumerable<TToken> expression)
        {
            List<TToken> output = new List<TToken>();
            Stack<TToken> stack = new Stack<TToken>();
            Stack<FunctionArgs> argsOccurence = new Stack<FunctionArgs>();

            bool commaOccured = false;

            foreach (var token in expression)
            {
                if (IsSkippable(token))
                    continue;

                if (IsOperator(token))
                {
                    while (!stack.IsEmpty() && IsOperator(stack.Peek()))
                    {
                        if (
                            (IsAssociative(token, Associativity.Left) && TestPrecedence(token, stack.Peek()) <= 0) ||
                            IsAssociative(token, Associativity.Right) && TestPrecedence(token, stack.Peek()) < 0)
                        {
                            output.Add(stack.Pop());
                            continue;
                        }
                        break;
                    }
                    stack.Push(token);
                }
                else if (IsLeftParenthesis(token))
                {
                    stack.Push(token);
                }
                else if (IsRightParenthesis(token))
                {
                    while (!stack.IsEmpty() && !IsLeftParenthesis(stack.Peek()))
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Pop();
                    if(!stack.IsEmpty() && IsFunction(stack.Peek()))
                    {
                        stack.Pop();

                        var fun = argsOccurence.Pop();
                        output.Add(RenameFunctionToHaveArgsCount(fun.Name, fun.ArgsCount + (commaOccured ? 1 : 0)));
                    }
                }
                else if (IsFunction(token))
                {
                    stack.Push(token);

                    commaOccured = false;
                    argsOccurence.Push(new FunctionArgs{ Name = token, ArgsCount = 0 });
                }
                else if(IsComma(token))
                {
                    commaOccured = true;
                    while(!stack.IsEmpty() && !IsLeftParenthesis(stack.Peek()))
                    {
                        output.Add(stack.Pop());
                    }
                    if(!argsOccurence.IsEmpty())
                    {
                        argsOccurence.Peek().ArgsCount += 1;
                    }
                }
                else
                {
                    output.Add(token);
                }
            }
            while (!stack.IsEmpty())
            {
                output.Add(stack.Pop());
            }

            return output.ToArray();
        }

        protected abstract bool IsComma(TToken token);
        protected abstract bool IsWord(TToken token);
        protected abstract bool IsSkippable(TToken token);

        protected abstract bool IsRightParenthesis(TToken token);
        protected abstract bool IsLeftParenthesis(TToken token);

        protected bool IsAssociative(TToken token, Associativity associativity) => IsOperator(token) && operators[token].Associativity == associativity;
        protected virtual bool IsOperator(TToken token) => operators.ContainsKey(token);

        protected int TestPrecedence(TToken token1, TToken token2)
        {
            if (IsOperator(token1) && IsOperator(token2))
            {
                return operators[token1].Weight - operators[token2].Weight;
            }
            throw new ArgumentException("One of arguments isn't operator");
        }

        private bool IsFunction(TToken token) => IsWord(token) && !IsOperator(token);

        protected abstract TToken RenameFunctionToHaveArgsCount(TToken oldFunctionToken, int argsCount);
    }
}
