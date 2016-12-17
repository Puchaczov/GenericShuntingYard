using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    internal class ChainedTaskAction : InternalTaskExecutor
    {
        private Action<TaskExecutionContext, ProgressChange> action;

        public ChainedTaskAction(Action<TaskExecutionContext, ProgressChange> action, TaskExecutionStarted delegateStarted, TaskExecutionStopped delegateStopped, ProgressChange delegateProgressChange)
            : base(delegateStarted, delegateStopped, delegateProgressChange)
        {
            this.action = action;
        }

        protected override Task Execute(TaskExecutionContext context, ProgressChange onProgressChange) => Task.Factory.StartNew(() 
            => action(context, onProgressChange));
    }
}
