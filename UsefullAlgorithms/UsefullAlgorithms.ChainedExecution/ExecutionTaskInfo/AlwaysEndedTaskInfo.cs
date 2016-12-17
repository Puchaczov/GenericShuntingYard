using System;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    internal class AlwaysEndedTaskInfo : ExecutionTaskInfo
    {
        public AlwaysEndedTaskInfo()
            : base(-1, null, null, null, ChainedTaskState.Ended)
        { }

        public override InternalTaskExecutor Create()
        {
            throw new NotSupportedException();
        }
    }
}