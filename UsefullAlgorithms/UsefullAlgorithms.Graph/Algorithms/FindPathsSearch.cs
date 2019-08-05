using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace UsefullAlgorithms.Graph.Algorithms
{
    public interface ISearch<T, TEdge> where T : IEquatable<T> where TEdge : Edge<T>
    {
        Route<T>[] FindRoutes(Graph<T, TEdge> graph, Vertex<T> startNode, Vertex<T> stopNode);
    }

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

            foreach (var vertex in graph)
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

    public class UnknownTopologySearch<T, TEdge> : ISearch<T, TEdge> where T : IEquatable<T> where TEdge : Edge<T>
    {
        public Route<T>[] FindRoutes(Graph<T, TEdge> graph, Vertex<T> startNode, Vertex<T> stopNode)
        {
            var visitedVertices = new HashSet<long>();

            if (startNode.Data.Equals(stopNode.Data))
                return new Route<T>[0];

            var flowNodes = new Stack<Vertex<T>>();

            flowNodes.Push(startNode);

            var traverseAlgorithm =
                new DefaultTraverseAlgorithFactory<T, Edge<T>>(DefaultTraverseAlgorithFactory<T, Edge<T>>.Algorithm.BreadthFirstSearch);

            var traverseTree = new Graph<T, Edge<T>>(traverseAlgorithm, startNode.Data);

            visitedVertices.Add(startNode.Id);

            while (flowNodes.Count > 0)
            {
                var currentNode = flowNodes.Pop();

                traverseTree.Add(currentNode);

                var adjacentNodes = graph.GetAdjacents(currentNode);

                foreach (var adjacentNode in adjacentNodes)
                {
                    if (visitedVertices.Contains(adjacentNode.Id))
                        continue;

                    traverseTree.Add(adjacentNode);
                    traverseTree.Connect(currentNode, adjacentNode, new Edge<T>(currentNode, adjacentNode));

                    flowNodes.Push(adjacentNode);
                }
            }

            var parents = new Dictionary<long, List<List<long>>>();
            var queue = new Queue<Vertex<T>>();
            queue.Enqueue(startNode);
            parents.Add(startNode.Id, new List<List<long>>() { new List<long>() });

            while(queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (!parents.ContainsKey(current.Id))
                    parents.Add(current.Id, new List<List<long>>() { new List<long>() });

                foreach(var adjacent in traverseTree.GetAdjacents(current))
                {
                    if (!parents.ContainsKey(adjacent.Id))
                    {
                        var parentCopy = new List<long>();
                        parentCopy.AddRange(parents[current.Id][0]);
                        parentCopy.Add(current.Id);
                        parents.Add(adjacent.Id, new List<List<long>>() { parentCopy });
                    }
                    else
                    {
                        var parentCopy = new List<long>();
                        parentCopy.AddRange(parents[current.Id][0]);
                        parentCopy.Add(current.Id);
                        parents[adjacent.Id].Add(parentCopy);
                    }
                    queue.Enqueue(adjacent);
                }
            }

            var rawRoutes = parents[stopNode.Id];
            var routes = new List<Route<T>>();

            foreach(var rawRoute in rawRoutes)
            {
                var vertices = new List<Vertex<T>>();
                foreach (var vertex in rawRoute)
                    vertices.Add(graph.GetById(vertex));

                vertices.Add(graph.GetById(stopNode.Id));

                var route = new Route<T>(vertices);
                routes.Add(route);
            }

            return routes.ToArray();
        }
    }

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
            StringBuilder builder = new StringBuilder();

            for(int i = 0; i < _vertices.Count - 1; ++i)
            {
                builder.Append(_vertices[i].Data.ToString());
                builder.Append(" => ");
            }
            builder.Append(_vertices[_vertices.Count - 1].Data.ToString());

            return builder.ToString();
        }
    }
}
