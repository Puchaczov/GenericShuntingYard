using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    internal class FuncExecutionTaskInfo : ExecutionTaskInfo
    {
        private Func<TaskExecutionContext, ProgressChange, Task> func;

        public FuncExecutionTaskInfo(Func<TaskExecutionContext, ProgressChange, Task> func, int id, TaskExecutionStarted delegateStarted, TaskExecutionStopped delegateStopped, ProgressChange delegateProgressChange, ChainedTaskState state = ChainedTaskState.WaitingToBeQueued)
            : base(id, delegateStarted, delegateStopped, delegateProgressChange, state)
        {
            this.func = func;
        }

        public override InternalTaskExecutor Create() => new ChainedTaskFunc(func, delegateStarted, delegateStopped, delegateProgressChange);
    }
}
