using System;
using System.Collections.Generic;
using System.Linq;

namespace UsefullAlgorithms.Graph.Algorithms
{
    public class DijkstraSearch<T, TEdge> : ISearch<T, TEdge> where T : IEquatable<T> where TEdge : WeightedEdge<T>
    {
        public Route<T>[] FindRoutes(Graph<T, TEdge> graph, Vertex<T> startNode, Vertex<T> stopNode)
        {
            var queue = new Queue<Vertex<T>>();
            var visitedVertices = new HashSet<long>();

            queue.Enqueue(startNode);
            visitedVertices.Add(startNode.Id);

            var vertices = new Dictionary<long, Vertex<T>>();
            var distances = new Dictionary<Vertex<T>, double>();
            var prevs = new Dictionary<Vertex<T>, Vertex<T>>();

            foreach (var vertex in graph.GetVertices())
            {
                vertices.Add(vertex.Id, vertex);
                distances.Add(vertex, double.MaxValue);
                prevs.Add(vertex, null);
            }

            distances[startNode] = 0;

            while(queue.Count > 0)
            {
                var vertice = queue.Dequeue();

                foreach (var adjacent in graph.GetAdjacents(vertice))
                {
                    var edge = graph.GetEdge(vertice, adjacent);
                    var currentSum = distances[vertice] + edge.Weight;

                    if (currentSum < distances[adjacent])
                    {
                        distances[adjacent] = currentSum;
                        prevs[adjacent] = vertice;
                    }

                    if (visitedVertices.Contains(adjacent.Id))
                        continue;

                    visitedVertices.Add(adjacent.Id);
                    queue.Enqueue(adjacent);
                }
            }

            var orderedPath = new LinkedList<Vertex<T>>();
            orderedPath.AddLast(stopNode);

            var pathVert = stopNode;
            while(prevs[pathVert] != startNode)
            {
                pathVert = prevs[pathVert];
                orderedPath.AddFirst(pathVert);
            }

            orderedPath.AddFirst(prevs[pathVert]);

            return new Route<T>[] { new Route<T>(orderedPath.ToList()) };
        }
    }
}
