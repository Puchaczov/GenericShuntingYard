using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.Graph
{
    public class VertexEqualityComparer<T> : IComparer<Vertex<T>>, IEqualityComparer<Vertex<T>> where T : IEquatable<T>
    {
        public int Compare(Vertex<T> x, Vertex<T> y) => x.CompareTo(y);

        public bool Equals(Vertex<T> x, Vertex<T> y) => x.Data.Equals(y.Data);

        public int GetHashCode(Vertex<T> obj) => obj.GetHashCode();
    }
}
