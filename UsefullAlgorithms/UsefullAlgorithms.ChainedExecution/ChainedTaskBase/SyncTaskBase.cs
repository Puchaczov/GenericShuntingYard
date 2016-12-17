using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    public abstract class SyncTaskBase : ChainedTaskBase
    {
        public override Task Execute(TaskExecutionContext context, ProgressChange onProgressChange)
            => Task.Factory.StartNew(() => Run(context, onProgressChange));

        public abstract void Run(TaskExecutionContext context, ProgressChange onProgressChange);
    }
}
