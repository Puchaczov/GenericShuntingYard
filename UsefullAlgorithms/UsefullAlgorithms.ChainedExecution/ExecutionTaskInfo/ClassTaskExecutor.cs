using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    internal class ClassTaskExecutor : ExecutionTaskInfo
    {
        private Type type;
        private object[] args;

        public ClassTaskExecutor(Type type, object[] args, int id, TaskExecutionStarted delegateStarted, TaskExecutionStopped delegateStopped, ProgressChange delegateProgressChange, ChainedTaskState state = ChainedTaskState.WaitingToBeQueued)
            : base(id, delegateStarted, delegateStopped, delegateProgressChange, state)
        {
            this.type = type;
            this.args = args;
        }

        public override InternalTaskExecutor Create() => new ChainedTaskClass(type, args, delegateStarted, delegateStopped, delegateProgressChange);
    }
}
