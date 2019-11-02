using System.Collections.Generic;
using System.Linq;

namespace UsefullAlgorithms.MarkovChains
{
    public abstract class MarkovChainTransitionMatrixModelTrainer<T>
    {
        public abstract MarkovChain<T> Train(T value);

        protected MarkovTransitionMatrix<T> FillOccurences(IDictionary<T, IReadOnlyCollection<T>> occurences)
        {
            var matrix = new MarkovTransitionMatrix<T>();
            var occurencesDict = new Dictionary<T, Dictionary<T, int>>();

            foreach (var keyRow in occurences)
            {
                occurencesDict.Add(keyRow.Key, new Dictionary<T, int>());

                foreach (var value in keyRow.Value)
                {
                    if (occurencesDict[keyRow.Key].ContainsKey(value))
                        occurencesDict[keyRow.Key][value] += 1;
                    else
                        occurencesDict[keyRow.Key].Add(value, 1);
                }
            }

            foreach (var row in occurencesDict)
            {
                var probabilityDictionary = new ProbablityDictionary<T>();

                matrix.Add(row.Key, probabilityDictionary);

                var sum = row.Value.Values.Sum();

                foreach (var column in row.Value)
                {
                    probabilityDictionary.Add(column.Key, (double)column.Value / sum);
                }
            }

            return matrix;
        }
    }
}
