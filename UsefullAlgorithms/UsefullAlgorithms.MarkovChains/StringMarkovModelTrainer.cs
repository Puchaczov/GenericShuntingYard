using System.Collections.Generic;
using System.Linq;

namespace UsefullAlgorithms.MarkovChains
{
    public class StringMarkovModelTrainer : MarkovChainTransitionMatrixModelTrainer<string>
    {
        public override MarkovChain<string> Train(string value)
        {
            var words = value.Split();
            
            var occurences = new Dictionary<string, List<string>>
            {
                { string.Empty, new List<string> { words[0] } }
            };
            
            for (int i = 0; i < words.Length - 1; ++i)
            {
                if (!occurences.ContainsKey(words[i]))
                {
                    occurences.Add(words[i], new List<string> { words[i + 1] });
                }
                else
                {
                    occurences[words[i]].Add(words[i + 1]);
                }
            }

            if (occurences.ContainsKey(words[words.Length - 1]))
            {
                occurences[words[words.Length - 1]].Add(string.Empty);
            }
            else
            {
                occurences.Add(words[words.Length - 1], new List<string> { string.Empty });
            }

            var model = FillOccurences(occurences.ToDictionary(f => f.Key, f => (IReadOnlyCollection<string>)f.Value));

            return new MarkovChain<string>(model, new SimpleRandom());
        }
    }
}
