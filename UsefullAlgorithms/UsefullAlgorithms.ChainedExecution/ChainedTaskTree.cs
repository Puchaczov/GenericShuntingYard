using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UsefullAlgorithms.Graph;

namespace UsefullAlgorithms.ChainedExecution
{
    internal class ChainedTaskTree : IEnumerable<ExecutionTaskInfo>
    {
        private Graph<ExecutionTaskInfo, DirectedEdge> graph;
        private ExecutionTaskInfo root;

        public int Count => graph.VerticlesCount;
        public int CountOfWaitingToBeQueued
        {
            get
            {
                int cnt = 0;
                var enumerator = graph.GetEnumerator(f => new BreadthFirstSearch<ExecutionTaskInfo, DirectedEdge>(f, root, true));

                while(enumerator.MoveNext())
                {
                    if(enumerator.Current.Data.State == ChainedTaskState.WaitingToBeQueued)
                        cnt += 1;
                }

                return cnt;
            }
        }

        public ChainedTaskTree()
        {
            this.graph = new Graph<ExecutionTaskInfo, DirectedEdge>(new DefaultTraverseAlgorithFactory<ExecutionTaskInfo, DirectedEdge>(DefaultTraverseAlgorithFactory<ExecutionTaskInfo, DirectedEdge>.Algorithm.BreadthFirstSearch), null);
        }

        public void SetRoot(ExecutionTaskInfo task)
        {
            root = task;
        }

        public IReadOnlyList<ExecutionTaskInfo> DescendantsOf(ExecutionTaskInfo parent) => null;

        public void Add(ExecutionTaskInfo task)
        {
            graph.Add(task);
        }

        public void Connect(ExecutionTaskInfo parent, ExecutionTaskInfo task)
        {
            graph.Connect(parent, task, new ParentToChildEdge(graph.GetByValue(parent), graph.GetByValue(task)));
            graph.Connect(task, parent, new ChildToParentEdge(graph.GetByValue(task), graph.GetByValue(parent)));
        }

        public void RemoveChild(ExecutionTaskInfo task)
        {
            graph.Remove(task);
        }

        public IEnumerable<ExecutionTaskInfo> GetChild(ExecutionTaskInfo parent) => graph.GetEdges(parent).Where(f => f.Direction == Direction.ParentToChild).Select(f => f.Destination.Data);

        public IEnumerable<ExecutionTaskInfo> GetParent(ExecutionTaskInfo child) => graph.GetEdges(child).Where(f => f.Direction == Direction.ChildToParent).Select(f => f.Destination.Data);

        public IEnumerator<ExecutionTaskInfo> Traverse(Func<Graph<ExecutionTaskInfo, DirectedEdge>, ExecutionTaskInfo, IEnumerator<ExecutionTaskInfo>> f) => f(graph, root);

        public IEnumerator<ExecutionTaskInfo> GetEnumerator() => new FreeJobsOnlyEnumerator(this, root);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
