using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.Graph
{
    public class Edge<T> where T: IEquatable<T>
    {
        public enum Relation
        {
            ParentToChild,
            ChildToParent,
            SelfConnected
        }

        public Vertex<T> Source { get; }
        public Vertex<T> Destination { get; }
        public Relation Direction { get; }

        public Edge(Vertex<T> source, Vertex<T> destination, Relation direction)
        {
            this.Source = source;
            this.Destination = destination;
            this.Direction = direction;
        }

        public Edge(T source, T destination)
        {
            this.Source = new Vertex<T>(source);
            this.Destination = new Vertex<T>(destination);
        }
    }
}
