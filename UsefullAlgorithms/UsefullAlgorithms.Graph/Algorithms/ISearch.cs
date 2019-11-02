using System;

namespace UsefullAlgorithms.Graph.Algorithms
{
    public interface ISearch<T, TEdge> where T : IEquatable<T> where TEdge : Edge<T>
    {
        Route<T>[] FindRoutes(Graph<T, TEdge> graph, Vertex<T> startNode, Vertex<T> stopNode);
    }
}
