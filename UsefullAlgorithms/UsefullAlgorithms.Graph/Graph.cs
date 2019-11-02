using System;
using System.Collections.Generic;
using System.Linq;

namespace UsefullAlgorithms.Graph
{

    public class Graph<T, TEdge> where T : IEquatable<T> where TEdge : Edge<T>
    {
        private readonly Dictionary<T, Vertex<T>> _hashCodeLookup;
        private readonly Dictionary<long, Vertex<T>> _idHashCodeLookup;
        private readonly Dictionary<Vertex<T>, LinkedList<Vertex<T>>> _vertices;
        private readonly Dictionary<Vertex<T>, LinkedList<TEdge>> _edges;

        public Graph()
        {
            _vertices = new Dictionary<Vertex<T>, LinkedList<Vertex<T>>>(new VertexEqualityComparer<T>());
            _edges = new Dictionary<Vertex<T>, LinkedList<TEdge>>(new VertexEqualityComparer<T>());
            _hashCodeLookup = new Dictionary<T, Vertex<T>>();
            _idHashCodeLookup = new Dictionary<long, Vertex<T>>();
        }

        public int VerticesCount => _vertices.Count;

        public int EdgesCount
        {
            get
            {
                var p = 0;

                foreach (var item in _edges)
                {
                    p += item.Value.Count;
                }

                return p;
            }
        }

        public IEnumerable<Vertex<T>> GetVertices()
        {
            return _hashCodeLookup.Values;
        }

        public IEnumerable<Vertex<T>> GetAdjacents(T data) => GetAdjacents(GetByValue(data));

        public IEnumerable<Vertex<T>> GetAdjacents(Vertex<T> vertex)
        {
            if (!_vertices.ContainsKey(vertex))
                throw new NotSupportedException();

            return _vertices[vertex];
        }

        public IEnumerable<TEdge> GetEdges(T data) => GetEdges(GetByValue(data));

        public IEnumerable<TEdge> GetEdges(Vertex<T> vertex)
        {
            if (!_vertices.ContainsKey(vertex))
                throw new NotSupportedException();

            return _edges[vertex];
        }

        public TEdge GetEdge(T first, T second)
        {
            if (!_hashCodeLookup.ContainsKey(first))
                return null;

            if (!_hashCodeLookup.ContainsKey(second))
                return null;

            return GetEdge(GetByValue(first), GetByValue(second));
        }

        public TEdge GetEdge(Vertex<T> first, Vertex<T> second)
        {
            if (!HasVertex(first))
                return null;

            return _edges[first].SingleOrDefault(f => f.Destination.Data.Equals(second.Data));
        }

        public void Add(Vertex<T> vertex)
        {
            if (!_vertices.ContainsKey(vertex))
                _vertices.Add(vertex, new LinkedList<Vertex<T>>());

            if (!_edges.ContainsKey(vertex))
                _edges.Add(vertex, new LinkedList<TEdge>());

            if (!_hashCodeLookup.ContainsKey(vertex.Data))
                _hashCodeLookup.Add(vertex.Data, vertex);

            if (!_idHashCodeLookup.ContainsKey(vertex.Id))
                _idHashCodeLookup.Add(vertex.Id, vertex);
        }

        public void Add(T item)
        {
            if (_hashCodeLookup.ContainsKey(item)) return;

            var v = new Vertex<T>(item);
            Add(v);
        }

        public void Add(params T[] items)
        {
            foreach (var item in items)
                Add(item);
        }

        public void Remove(T data)
        {
            Remove(GetByValue(data));
        }

        public void Remove(Vertex<T> vertex)
        {
            if (!HasVertex(vertex))
                return;

            foreach(var item in _vertices)
            {
                if(item.Value.Contains(vertex))
                    item.Value.Remove(vertex);
            }

            _vertices.Remove(vertex);
            _edges.Remove(vertex);

            _hashCodeLookup.Remove(vertex.Data);

            _idHashCodeLookup.Remove(vertex.Id);

            var e = _edges.SelectMany(f => f.Value.Where(x => x.Destination.Equals(vertex))).ToArray();
            
            foreach(var edge in e)
            {
                _edges[edge.Source].Remove(edge);
            }
        }

        public void Remove(TEdge edge)
        {
            _edges[edge.Source].Remove(edge);
            _edges[edge.Destination].Remove(edge);
        }

        public void Connect(Vertex<T> from, Vertex<T> to, TEdge edge)
        {
            if (!_vertices.ContainsKey(from))
                throw new InvalidOperationException();

            if (HasVertex(_vertices[from], to))
                throw new InvalidOperationException();

            if (edge.Source != from)
                throw new InvalidOperationException();

            if (edge.Destination != to)
                throw new InvalidOperationException();

            _vertices[from].AddLast(to);
            _edges[from].AddLast(edge);
        }

        public void Connect(T from, T to, TEdge edge)
        {
            var first = GetByValue(from);
            var second = GetByValue(to);

            if (first != null && second != null)
            {
                if (GetEdge(first, second) != null)
                    return;

                Connect(first, second, edge);
            }
        }

        private bool HasVertex(ICollection<Vertex<T>> vertices, Vertex<T> vertex) => vertices.Contains(vertex);

        public bool HasVertex(Vertex<T> vertex) => _hashCodeLookup.ContainsKey(vertex.Data);

        public bool HasVertex(T data) => _hashCodeLookup.ContainsKey(data);

        public bool HasEdge(T source, T destination) => HasEdge(GetByValue(source), GetByValue(destination));

        public bool HasEdge(Vertex<T> source, Vertex<T> destination) => _edges[source].Any(f => f.Destination.Data.Equals(destination.Data));

        public Vertex<T> GetByValue(T value) => _hashCodeLookup[value];

        public Vertex<T> GetById(long id) => _idHashCodeLookup[id];

        public bool HasCycle(Vertex<T> point)
        {
            if (!HasVertex(point))
                return false;

            var s = new Stack<Vertex<T>>();
            var visited = new SortedSet<Vertex<T>>();

            s.Push(point);
            while(s.Count > 0)
            {
                var node = s.Pop();

                foreach (var desc in GetAdjacents(node))
                {
                    if (node.Data.Equals(desc.Data))
                        return true;

                    if (visited.Any(f => f.Data.Equals(desc.Data)))
                        return true;

                    if (s.Any(f => f.Data.Equals(desc.Data)))
                        return true;

                    s.Push(desc);
                }

                visited.Add(node);
            }

            return false;
        }
    }
}
