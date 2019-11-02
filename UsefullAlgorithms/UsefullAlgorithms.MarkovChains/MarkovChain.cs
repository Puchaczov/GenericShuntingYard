using System;
using System.Collections.Generic;

namespace UsefullAlgorithms.MarkovChains
{
    public class MarkovChain<T>
    {
        private readonly MarkovTransitionMatrix<T> _transitionMatrix;
        private readonly IRandom _random;

        public MarkovChain(MarkovTransitionMatrix<T> model, IRandom random)
        {
            _transitionMatrix = model;
            _random = random;
        }

        public IEnumerable<T> Generate(T initialState, Func<int, T, bool> continueWhile)
        {
            yield return initialState;

            var state = initialState;
            var time = 0;

            while (continueWhile(time++, state))
            {
                var newState = Choose(state);

                yield return newState;

                state = newState;
            }
        }

        private T Choose(T state)
        {
            return _transitionMatrix[state].GetForProbability(_random.Next());
        }
    }
}
