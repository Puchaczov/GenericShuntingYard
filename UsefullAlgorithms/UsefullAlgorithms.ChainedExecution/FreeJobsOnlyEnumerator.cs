using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UsefullAlgorithms.ChainedExecution
{
    internal class FreeJobsOnlyEnumerator : IEnumerator<ExecutionTaskInfo>
    {
        private readonly ExecutionTaskInfo root;
        private ExecutionTaskInfo start;
        private readonly ChainedTaskTree tree;

        private Queue<ExecutionTaskInfo> jobStack;

        public FreeJobsOnlyEnumerator(ChainedTaskTree tree, ExecutionTaskInfo root)
        {
            this.root = root;
            this.tree = tree;
            jobStack = new Queue<ExecutionTaskInfo>();

            Reset();
        }

        public ExecutionTaskInfo Current
        {
            get;
            private set;
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        { }

        public bool MoveNext()
        {
            Current = null;

            while (jobStack.Count > 0)
            {
                var node = jobStack.Dequeue();

                foreach (var dsc in tree.GetChild(node).Where(f => f.State == ChainedTaskState.WaitingToBeQueued || f.State == ChainedTaskState.Ended))
                    if(!jobStack.Contains(dsc))
                        jobStack.Enqueue(dsc);

                if (IsJobFree(node))
                {
                    Current = node;
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            jobStack.Clear();
            start = root;
            jobStack.Enqueue(root);
        }

        private bool IsJobFree(ExecutionTaskInfo node)
        {
            var graph = this.tree.GetParent(node);

            return node.State == ChainedTaskState.WaitingToBeQueued && graph.All(f => f.State == ChainedTaskState.Ended);
        }
    }
}