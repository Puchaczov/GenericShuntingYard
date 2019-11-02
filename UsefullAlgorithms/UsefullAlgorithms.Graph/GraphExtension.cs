using System;
using System.Collections.Generic;

namespace UsefullAlgorithms.Graph
{
    public static class GraphExtension
    {
        public static IEnumerable<Vertex<T>> GetEnumerable<T, TEdge>(this Graph<T, TEdge> graph)
            where T : IEquatable<T> where TEdge : Edge<T>
        {
            return null;
        }

        public static IEnumerator<Vertex<T>> GetEnumerator<T, TEdge>(this Graph<T, TEdge> graph)
            where T : IEquatable<T> where TEdge : Edge<T>
        {
            return null;
        }

        public static IEnumerator<Vertex<T>> GetEnumerator<T, TEdge>(this Graph<T, TEdge> graph, Func<Graph<T, TEdge>, IEnumerator<Vertex<T>>> func)
            where T : IEquatable<T> where TEdge : Edge<T>
        {
            return func(graph);
        }
    }
}
