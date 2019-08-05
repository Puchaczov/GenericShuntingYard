using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.Graph
{
    public class WeightedEdge<T> : Edge<T> where T : IEquatable<T>
    {
        public WeightedEdge(double weight, Vertex<T> source, Vertex<T> destination)
            : base(source, destination)
        {
            Weight = weight;
        }

        public double Weight { get; }
    }
}
