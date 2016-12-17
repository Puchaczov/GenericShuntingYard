using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    public abstract class AsyncTaskBase : ChainedTaskBase
    {
        public override Task Execute(TaskExecutionContext context, ProgressChange onProgressChange)
            => Run(context, onProgressChange);

        public abstract Task Run(TaskExecutionContext context, ProgressChange onProgressChange);
    }
}
