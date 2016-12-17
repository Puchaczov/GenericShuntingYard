using System;
using UsefullAlgorithms.Graph;

namespace UsefullAlgorithms.ChainedExecution
{
    enum Direction
    {
        ParentToChild,
        ChildToParent,
        Bidirectional
    }

    internal abstract class DirectedEdge : Edge<ExecutionTaskInfo>
    {
        public DirectedEdge(Vertex<ExecutionTaskInfo> source, Vertex<ExecutionTaskInfo> destination)
            : base(source, destination)
        { }

        public abstract Direction Direction { get; }
    }

    internal class ParentToChildEdge : DirectedEdge
    {
        public ParentToChildEdge(Vertex<ExecutionTaskInfo> source, Vertex<ExecutionTaskInfo> destination)
            : base(source, destination)
        { }

        public override Direction Direction => Direction.ParentToChild;
    }

    internal class ChildToParentEdge : DirectedEdge
    {
        public ChildToParentEdge(Vertex<ExecutionTaskInfo> source, Vertex<ExecutionTaskInfo> destination)
            : base(source, destination)
        { }

        public override Direction Direction => Direction.ChildToParent;
    }
}