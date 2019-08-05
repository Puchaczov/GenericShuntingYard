using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefullAlgorithms.Graph.Algorithms;

namespace UsefullAlgorithms.Graph
{
    public class DefaultTraverseAlgorithFactory<T, TEdge> : ITraverseAlgorithmFactory<T, TEdge, Vertex<T>> where T : IEquatable<T> where TEdge : Edge<T>
    {
        public enum Algorithm
        {
            BreadthFirstSearch,
            DepthFirstSearch
        }

        private Algorithm algorithm;

        public DefaultTraverseAlgorithFactory(Algorithm alg)
        {
            this.algorithm = alg;
        }

        public IEnumerator<Vertex<T>> Create(Graph<T, TEdge> graph, T root)
        {
            switch(algorithm)
            {
                case Algorithm.BreadthFirstSearch:
                    return new BreadthFirstSearch<T, TEdge>(graph, root, true);
                case Algorithm.DepthFirstSearch:
                    return new DepthFirstSearch<T, TEdge>(graph, root, true);
            }
            throw new NotSupportedException(nameof(algorithm));
        }
    }
}
