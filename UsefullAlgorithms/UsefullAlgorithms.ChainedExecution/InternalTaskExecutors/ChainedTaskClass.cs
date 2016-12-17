using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    internal class ChainedTaskClass : InternalTaskExecutor
    {
        private Type type;
        private object[] args;

        public ChainedTaskClass(Type type, object[] args, TaskExecutionStarted delegateStarted, TaskExecutionStopped delegateStopped, ProgressChange progressChange) 
            : base(delegateStarted, delegateStopped, progressChange)
        {
            this.type = type;
            this.args = args;
        }

        protected override Task Execute(TaskExecutionContext context, ProgressChange onProgressChange)
            => CreateTask().Execute(context, onProgressChange);

        private ChainedTaskBase CreateTask() 
            => (ChainedTaskBase)Activator.CreateInstance(type, args);
    }
}
