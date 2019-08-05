using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.Graph
{
    [DebuggerDisplay("{Source.Data.ToString()} => {Destination.Data.ToString()}")]
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

        public Edge(Vertex<T> source, Vertex<T> destination)
        {
            this.Source = source;
            this.Destination = destination;
        }

        public Edge(T source, T destination)
        {
            this.Source = new Vertex<T>(source);
            this.Destination = new Vertex<T>(destination);
        }
    }
}
