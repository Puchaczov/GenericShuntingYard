using System;
using System.Diagnostics;

namespace UsefullAlgorithms.Graph
{

    [DebuggerDisplay("{Data.ToString()}")]
    public class Vertex<T> : IComparable<Vertex<T>>, IComparable where T : IEquatable<T>
    {
        private static long IdSeed;
        private static readonly object IdGuard = new object();

        public long Id { get; }

        public T Data { get; }

        protected Vertex(T data, long id)
        {
            Data = data;
            Id = id;
        }

        public Vertex(T data)
        {
            Data = data;
            lock (IdGuard)
                Id = IdSeed++;
        }

        public int CompareTo(Vertex<T> other)
        {
            if (Id == other.Id)
                return 1;
            return -1;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is Vertex<T>)
                return CompareTo((Vertex<T>)obj);

            throw new ArgumentException($"Object is not of type {nameof(T)}");
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
