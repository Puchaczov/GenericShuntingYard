using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.Graph
{
    public class DirectedEdge<T> : Edge<T> where T : IEquatable<T>
    {
        private Relation Direction { get; }

        public DirectedEdge(Vertex<T> source, Vertex<T> destination, Relation direction)
            : base(source, destination)
        {
            Direction = direction;
        }
    }
}
