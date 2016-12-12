using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UsefullAlgorithms.Graph
{
    public class Graph<T, TEdge> : IEnumerable<Vertex<T>> where T : IEquatable<T> where TEdge : Edge<T>
    {
        private Dictionary<T, Vertex<T>> hashCodeLookup;
        private Dictionary<Vertex<T>, LinkedList<Vertex<T>>> verticles;
        private Dictionary<Vertex<T>, LinkedList<TEdge>> edges;
        private ITraverseAlgorithmFactory<T, TEdge, Vertex<T>> traverseAlgorithm;
        private T startPoint;

        public Graph(ITraverseAlgorithmFactory<T, TEdge, Vertex<T>> traverseAlgorithm, T startPoint)
        {
            verticles = new Dictionary<Vertex<T>, LinkedList<Vertex<T>>>(new VertexEqualityComparer<T>());
            edges = new Dictionary<Vertex<T>, LinkedList<TEdge>>(new VertexEqualityComparer<T>());
            hashCodeLookup = new Dictionary<T, Vertex<T>>();
            this.traverseAlgorithm = traverseAlgorithm;
            this.startPoint = startPoint;
        }

        public T StartPoint
        {
            get
            {
                return startPoint;
            }
            set
            {
                startPoint = value;
            }
        }

        public ITraverseAlgorithmFactory<T, TEdge, Vertex<T>> TraverseAlgorithm
        {
            get
            {
                return traverseAlgorithm;
            }
            set
            {
                traverseAlgorithm = value;
            }
        }

        public int VerticlesCount => verticles.Count;

        public int EdgesCount
        {
            get
            {
                var p = 0;

                foreach (var item in edges)
                {
                    p += item.Value.Count;
                }

                return p;
            }
        }

        public IEnumerable<Vertex<T>> GetAdjacents(T data) => GetAdjacents(GetByValue(data));

        public IEnumerable<Vertex<T>> GetAdjacents(Vertex<T> vertex)
        {
            if (!verticles.ContainsKey(vertex))
                throw new NotSupportedException();

            return verticles[vertex];
        }

        public IEnumerable<TEdge> GetEdges(T data) => GetEdges(GetByValue(data));

        public IEnumerable<TEdge> GetEdges(Vertex<T> vertex)
        {
            if (!verticles.ContainsKey(vertex))
                throw new NotSupportedException();

            return edges[vertex];
        }

        public TEdge GetEdge(T first, T second)
        {
            if (!hashCodeLookup.ContainsKey(first))
                return default(TEdge);

            if (!hashCodeLookup.ContainsKey(second))
                return default(TEdge);

            return GetEdge(GetByValue(first), GetByValue(second));
        }

        public TEdge GetEdge(Vertex<T> first, Vertex<T> second)
        {
            if (!HasVertex(first))
                return null;

            return edges[first].SingleOrDefault(f => f.Destination.Data.Equals(second.Data));
        }

        public void Add(Vertex<T> vertex)
        {
            if (!verticles.ContainsKey(vertex))
                verticles.Add(vertex, new LinkedList<Vertex<T>>());

            if (!edges.ContainsKey(vertex))
                edges.Add(vertex, new LinkedList<TEdge>());

            if (!hashCodeLookup.ContainsKey(vertex.Data))
                hashCodeLookup.Add(vertex.Data, vertex);
        }

        public Vertex<T> Add(T item)
        {
            if (hashCodeLookup.ContainsKey(item))
                return hashCodeLookup[item];

            var v = new Vertex<T>(item);
            Add(v);

            return v;
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

            foreach(var item in verticles)
            {
                if(item.Value.Contains(vertex))
                    item.Value.Remove(vertex);
            }

            verticles.Remove(vertex);
            edges.Remove(vertex);

            hashCodeLookup.Remove(vertex.Data);

            var e = edges.SelectMany(f => f.Value.Where(x => x.Destination.Equals(vertex))).ToArray();
            
            foreach(var edge in e)
            {
                edges[edge.Source].Remove(edge);
            }
        }

        public void Remove(TEdge edge)
        {
            edges[edge.Source].Remove(edge);
            edges[edge.Destination].Remove(edge);
        }

        public void Connect(Vertex<T> from, Vertex<T> to, TEdge edge)
        {
            if (!verticles.ContainsKey(from))
                throw new InvalidOperationException();

            if (HasVerticle(verticles[from], to))
                throw new InvalidOperationException();

            if (edge.Source != from)
                throw new InvalidOperationException();

            if (edge.Destination != to)
                throw new InvalidOperationException();

            verticles[from].AddLast(to);
            edges[from].AddLast(edge);
        }

        public void Connect(T from, T to, TEdge edge)
        {
            var first = GetByValue(from);
            var second = GetByValue(to);

            if (first != null && second != null)
            {
                Connect(first, second, edge);
            }
        }

        private bool HasVerticle(LinkedList<Vertex<T>> vertices, Vertex<T> vertex) => vertices.Contains(vertex);

        public bool HasVertex(Vertex<T> v) => hashCodeLookup.ContainsKey(v.Data);

        public bool HasVertex(T data) => hashCodeLookup.ContainsKey(data);

        public bool HasEdge(T source, T Destination) => HasEdge(GetByValue(source), GetByValue(Destination));

        public bool HasEdge(Vertex<T> source, Vertex<T> destination) => edges[source].Any(f => f.Destination.Data.Equals(destination.Data));

        public Vertex<T> GetByValue(T value) => hashCodeLookup[value];

        public bool HasCycle(Vertex<T> point)
        {
            if (!HasVertex(point))
                return false;

            Stack<Vertex<T>> s = new Stack<Vertex<T>>();
            SortedSet<Vertex<T>> visited = new SortedSet<Vertex<T>>();

            s.Push(point);
            while(s.Count > 0)
            {
                var node = s.Pop();

                foreach (var desc in this.GetAdjacents(node))
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

        public IEnumerator<Vertex<T>> GetEnumerator(Func<Graph<T, TEdge>, IEnumerator<Vertex<T>>> f) => f(this);

        public IEnumerator<Vertex<T>> TraverseBy(T startFrom, Func<Graph<T, TEdge>, T, IEnumerator<Vertex<T>>> f) => f(this, startFrom);

        public IEnumerator<Vertex<T>> GetEnumerator() => traverseAlgorithm.Create(this, startPoint);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
