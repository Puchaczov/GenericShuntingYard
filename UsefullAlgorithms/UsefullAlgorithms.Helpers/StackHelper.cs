using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsefullAlgorithms.Helpers
{
    public static class StackHelper
    {
        public static bool IsEmpty<T>(this Stack<T> stack) => stack.Count == 0;

        public static T Swap<T>(this Stack<T> stack, T arg)
        {
            var poped = stack.Pop();
            stack.Push(arg);
            return poped;
        }
    }
}
