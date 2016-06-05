using System;
using System.Collections;
using System.Collections.Generic;
using UsefullAlgorithms.Helpers;

namespace UsefullAlgorithms.Parsing.ExpressionParsing
{
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

        private class Args
        {
            public int ArgsCount { get; set; }
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

        /// <summary>
        /// 1. Notice: Handling (1,2,3,4) anounymous syntax is a bit tricky. When we discover that "(" occur without function before, 
        /// There will be generated virtual function so it will occur like 'virtual(1,2,3,4)'
        /// 2. Notice: To check how much args appear in function, there is need to check for ',' occurences and add + 1 to current function arguments occurence. 
        ///            However, we can't trust this measurement when function contains only one parameter. To handle this, simple check
        ///            is your current token is inside function and if it's value, that means there is at least one argument.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected TToken[] InfixToPostfix(IEnumerable<TToken> expression)
        {
            List<TToken> output = new List<TToken>();
            Stack<TToken> stack = new Stack<TToken>();
            Stack<Args> argsOccurence = new Stack<Args>();

            Stack<bool> commaOccured = new Stack<bool>();
            Stack<bool> hasArguments = new Stack<bool>();

            TToken lastToken = default(TToken);

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
                    var isFunction = !stack.IsEmpty() && this.IsFunction(stack.Peek());
                    if (isFunction)
                        argsOccurence.Push(new Args { ArgsCount = 0 });
                    else
                        argsOccurence.Push(new Args { ArgsCount = 0 });

                    //if any left parenthesis occur inside function, I will assume that there is at least one argument
                    if (!hasArguments.IsEmpty())
                        hasArguments.Swap(true);    
                    hasArguments.Push(false);

                    stack.Push(token);
                    commaOccured.Push(false);
                }
                else if (IsRightParenthesis(token))
                {
                    while (!stack.IsEmpty() && !IsLeftParenthesis(stack.Peek()))
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Pop();

                    var hasArgs = hasArguments.Pop();
                    if(!stack.IsEmpty() && IsFunction(stack.Peek())) //function call ie. fun(1,2,3);
                    {
                        var fun = argsOccurence.Pop();
                        //generate vararg token to instruct evaluator how many arguments should be poped from stack
                        output.Add(GenerateVarArgToken(fun.ArgsCount + (commaOccured.Pop() || hasArgs ? 1 : 0)));
                        output.Add(GenerateFunctionToken(stack.Pop()));
                    }
                    else if(!stack.IsEmpty() && !IsFunction(stack.Peek())) // not a function ie. in (1,2,3)
                    {
                        var fun = argsOccurence.Pop();
                        if (commaOccured.Peek()) //only when "," occured in (...)
                            output.Add(GenerateVarArgToken(fun.ArgsCount + (commaOccured.Pop() || hasArgs ? 1 : 0)));
                        else
                            commaOccured.Pop();
                    }
                    else if(stack.IsEmpty() && hasArgs)
                    {
                        var fun = argsOccurence.Pop();
                        if (commaOccured.Peek())
                            //generate vararg that that isn't associated to any function. It's pure (1,2,3,...,n)
                            output.Add(GenerateVarArgToken(fun.ArgsCount + (commaOccured.Pop() || hasArgs ? 1 : 0)));
                        else
                            commaOccured.Pop();
                    }
                }
                else if (IsFunction(token))
                {
                    stack.Push(token);
                }
                else if(IsComma(token))
                {
                    commaOccured.Swap(true);
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
                    if (!hasArguments.IsEmpty() && !hasArguments.Peek())
                        hasArguments.Swap(true);
                    output.Add(token);
                }
                lastToken = token;
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
        protected abstract bool IsValue(TToken token);

        protected bool IsAssociative(TToken token, Associativity associativity) => IsOperator(token) && operators[token].Associativity == associativity;
        protected virtual bool IsOperator(TToken token) => operators.ContainsKey(token);

        private bool IsFunction(TToken token) => IsWord(token) && !IsOperator(token) && !IsValue(token);

        protected int TestPrecedence(TToken token1, TToken token2)
        {
            if (IsOperator(token1) && IsOperator(token2))
            {
                return operators[token1].Weight - operators[token2].Weight;
            }
            throw new ArgumentException("One of arguments isn't operator");
        }

        protected abstract TToken GenerateVarArgToken(int argsCount);
        protected abstract TToken GenerateFunctionToken(TToken oldToken);
    }
}
