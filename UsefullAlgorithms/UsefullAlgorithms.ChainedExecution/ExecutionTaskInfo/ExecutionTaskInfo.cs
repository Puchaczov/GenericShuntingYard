using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    internal abstract class ExecutionTaskInfo : IEquatable<ExecutionTaskInfo>
    {
        private readonly int id;
        private int state;
        protected TaskExecutionStarted delegateStarted;
        protected TaskExecutionStopped delegateStopped;
        protected ProgressChange delegateProgressChange;

        public ExecutionTaskInfo(int id, TaskExecutionStarted delegateStarted, TaskExecutionStopped delegateStopped, ProgressChange delegateProgressChange, ChainedTaskState state = ChainedTaskState.WaitingToBeQueued)
        {
            this.id = id;
            this.state = (int)state;
            this.delegateStarted = delegateStarted;
            this.delegateStopped = delegateStopped;
            this.delegateProgressChange = delegateProgressChange;
        }

        public int Id => id;

        public abstract InternalTaskExecutor Create();

        public ChainedTaskState State
        {
            get
            {
                return (ChainedTaskState)Interlocked.Exchange(ref state, state);
            }
            set
            {
                Interlocked.Exchange(ref state, (int)value);
            }
        }

        public bool Equals(ExecutionTaskInfo other) => id == other.id;
    }
}
