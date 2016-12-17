using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    internal class ChainedTaskFunc : InternalTaskExecutor
    {
        private Func<TaskExecutionContext, ProgressChange, Task> func;

        public ChainedTaskFunc(Func<TaskExecutionContext, ProgressChange, Task> func, TaskExecutionStarted delegateStarted, TaskExecutionStopped delegateStopped, ProgressChange delegateProgressChange)
            : base(delegateStarted, delegateStopped, delegateProgressChange)
        {
            this.func = func;
        }

        protected override Task Execute(TaskExecutionContext context, ProgressChange onProgressChange) 
            => func(context, onProgressChange);
    }
}
