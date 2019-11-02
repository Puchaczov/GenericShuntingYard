using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UsefullAlgorithms.MarkovChains;

namespace UserfullAlgorithms.MarkovChains.Tests
{
    [TestClass]
    public class MarkovChainTests
    {
        [TestMethod]
        public void MarkovChainTransitionMatrixTest()
        {
            var markovChainTrainver = new MarkovChainTransitionMatrixModelTestTrainer<string>();

            var model = markovChainTrainver.FillOccurences(new Dictionary<string, IReadOnlyCollection<string>> 
            {
                {
                    "test", 
                    new List<string>
                    {
                        "test1",
                        "test2",
                        "test2"
                    } 
                }
            });

            Assert.AreEqual("test2", model["test"].GetForProbability((double)2 / 3));
            Assert.AreEqual("test1", model["test"].GetForProbability((double)1 / 3));
        }
    }

    public class MarkovChainTransitionMatrixModelTestTrainer<T> : MarkovChainTransitionMatrixModelTrainer<T>
    {
        public override MarkovChain<T> Train(T value)
        {
            throw new System.NotImplementedException();
        }

        public new MarkovTransitionMatrix<T> FillOccurences(IDictionary<T, IReadOnlyCollection<T>> occurences)
        {
            return base.FillOccurences(occurences);
        }
    }
}
