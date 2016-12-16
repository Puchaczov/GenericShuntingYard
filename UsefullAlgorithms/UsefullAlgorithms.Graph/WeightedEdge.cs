using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.Graph
{
    public class WeightedEdge<T> : Edge<T> where T : IEquatable<T>
    {
        private readonly int weight;

        public WeightedEdge(int weight, Vertex<T> source, Vertex<T> destination, Relation direction)
            : base(source, destination, direction)
        {
            this.weight = weight;
        }

        public int Weight => weight;
    }
}
