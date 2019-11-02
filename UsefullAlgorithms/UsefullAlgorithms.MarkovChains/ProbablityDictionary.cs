using System;
using System.Collections.Generic;
using System.Linq;

namespace UsefullAlgorithms.MarkovChains
{
    public class ProbablityDictionary<T> : Dictionary<T, double>
    {
        private readonly List<(double Min, double Max, T Key)> _ranges = new List<(double, double, T)>();

        public new void Add(T key, double value)
        {
            base.Add(key, value);
            if (_ranges.Count == 0)
            {
                _ranges.Add((0, value, key));
            }
            else
            {
                _ranges.Add((_ranges[_ranges.Count - 1].Max, _ranges[_ranges.Count - 1].Max + value, key));
            }
        }

        public T GetForProbability(double value)
        {
            for (int i = 0; i < _ranges.Count; ++i)
            {
                if (IsWithinRange(_ranges[i], value)) 
                {
                    return _ranges[i].Key;
                }
            }

            throw new Exception("Value is outside of a range.");
        }

        private bool IsWithinRange((double Min, double Max, T Key) rangeKeyPair, double value)
        {
            return value >= rangeKeyPair.Min && value <= rangeKeyPair.Max;
        }
    }
}
