﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UsefullAlgorithms.Graph.Algorithms
{
    public class BreadthFirstSearch<T, TEdge> : IEnumerator<Vertex<T>> where T : IEquatable<T> where TEdge : Edge<T>
    {
        private readonly Graph<T, TEdge> graph;
        private readonly Vertex<T> node;
        private readonly Queue<Vertex<T>> queue;
        private readonly List<Vertex<T>> visited;
        private readonly bool ignoreCycles;

        public BreadthFirstSearch(Graph<T, TEdge> graph, T root, bool ignoreCycles = true)
        {
            this.graph = graph;
            this.node = graph.GetByValue(root);
            this.queue = new Queue<Vertex<T>>();
            this.visited = new List<Vertex<T>>();
            this.ignoreCycles = ignoreCycles;

            this.Reset();
        }

        public Vertex<T> Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext()
        {
            Current = null;

            while (queue.Count > 0)
            {
                var vert = queue.Dequeue();

                foreach(var desc in graph.GetAdjacents(vert))
                {
                    if (vert.Data.Equals(desc.Data))
                        continue;

                    if (ignoreCycles && visited.Any(f => f.Data.Equals(desc.Data)))
                        continue;

                    if (queue.Any(f => f.Data.Equals(desc.Data)))
                        continue;

                    queue.Enqueue(desc);
                }

                visited.Add(vert);
                Current = vert;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            Current = node;
            queue.Clear();
            visited.Clear();
            queue.Enqueue(Current);
        }
    }
}
