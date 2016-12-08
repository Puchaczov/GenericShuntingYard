using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UsefullAlgorithms.Graph
{
    public class DepthFirstSearch<T, TEdge> : IEnumerator<Vertex<T>> where T : IEquatable<T> where TEdge : Edge<T>
    {
        private readonly Graph<T, TEdge> graph;

        private readonly Vertex<T> node;
        private Vertex<T> current;
        private readonly Stack<Vertex<T>> stack;
        private readonly List<Vertex<T>> visited;
        private readonly bool ignoreCycles;

        public DepthFirstSearch(Graph<T, TEdge> graph, T root, bool v)
        {
            this.graph = graph;
            this.node = graph.GetByValue(root);
            stack = new Stack<Vertex<T>>();
            visited = new List<Vertex<T>>();
            this.ignoreCycles = v;

            Reset();
        }

        public Vertex<T> Current => current;

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext()
        {
            current = null;

            while(stack.Count > 0)
            {
                var vert = stack.Pop();

                foreach(var desc in graph.GetAdjacents(vert))
                {
                    if (vert.Data.Equals(desc.Data))
                        continue;

                    if (ignoreCycles && visited.Any(f => f.Data.Equals(desc.Data)))
                        continue;

                    if (stack.Any(f => f.Data.Equals(desc.Data)))
                        continue;

                    stack.Push(desc);
                }

                visited.Add(vert);
                current = vert;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            current = node;
            stack.Clear();
            visited.Clear();
            stack.Push(current);
        }
    }
}