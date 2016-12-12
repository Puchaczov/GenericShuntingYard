using System;
using System.Diagnostics;

namespace UsefullAlgorithms.Graph
{
    [DebuggerDisplay("{Data.ToString()}")]
    public class Vertex<T> : IComparable<Vertex<T>>, IComparable where T : IEquatable<T>
    {
        public T Data { get; }

        public Vertex(T data)
        {
            this.Data = data;
        }

        public int CompareTo(Vertex<T> other)
        {
            if (Data.Equals(other.Data))
                return 1;
            else
                return -1;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is Vertex<T>)
                return CompareTo((Vertex<T>)obj);

            throw new ArgumentException($"Object is not of type {nameof(T)}");
        }
    }
}
