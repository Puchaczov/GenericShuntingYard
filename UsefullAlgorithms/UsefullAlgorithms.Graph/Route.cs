using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace UsefullAlgorithms.Graph
{

    [DebuggerDisplay("{GetDebugString()}")]
    public class Route<T> : IEnumerable<Vertex<T>> where T : IEquatable<T>
    {
        private readonly List<Vertex<T>> _vertices;

        public Route(IReadOnlyList<Vertex<T>> vertices)
        {
            _vertices = vertices.ToList();
        }

        public IEnumerator<Vertex<T>> GetEnumerator()
        {
            return _vertices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string GetDebugString()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < _vertices.Count - 1; ++i)
            {
                builder.Append(_vertices[i].Data.ToString());
                builder.Append(" => ");
            }
            builder.Append(_vertices[_vertices.Count - 1].Data.ToString());

            return builder.ToString();
        }
    }
}
