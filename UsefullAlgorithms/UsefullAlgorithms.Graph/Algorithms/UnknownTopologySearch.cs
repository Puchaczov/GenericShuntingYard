using System;
using System.Collections.Generic;

namespace UsefullAlgorithms.Graph.Algorithms
{
    public class UnknownTopologySearch<T, TEdge> : ISearch<T, TEdge> where T : IEquatable<T> where TEdge : Edge<T>
    {
        public Route<T>[] FindRoutes(Graph<T, TEdge> graph, Vertex<T> startNode, Vertex<T> stopNode)
        {
            var visitedVertices = new HashSet<long>();

            if (startNode.Data.Equals(stopNode.Data))
                return new Route<T>[0];

            var flowNodes = new Stack<Vertex<T>>();

            flowNodes.Push(startNode);

            var traverseTree = new Graph<T, Edge<T>>();

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
}
