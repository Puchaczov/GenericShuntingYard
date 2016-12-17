using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    public abstract class ChainedTaskBase
    {
        public abstract Task Execute(TaskExecutionContext context, ProgressChange onProgressChange);
    }
}
