using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    public delegate void ProgressChange(int progress);
    public delegate void TaskExecutionStarted();
    public delegate void TaskExecutionStopped();

    internal abstract class InternalTaskExecutor
    {
        public event ProgressChange OnProgressChange;
        public event TaskExecutionStarted OnTaskStarted;
        public event TaskExecutionStopped OnTaskStopped;

        protected InternalTaskExecutor(TaskExecutionStarted delegateStarted, TaskExecutionStopped delegateStopped, ProgressChange progressChange)
        {
            OnTaskStarted += delegateStarted;
            OnTaskStopped += delegateStopped;
            OnProgressChange += progressChange;
        }

        public Task Execute(TaskExecutionContext context)
        {
            try
            {
                OnTaskStarted();

                if (context.CancellationToken.IsCancellationRequested)
                    context.CancellationToken.ThrowIfCancellationRequested();

                var task = Execute(context, OnProgressChange);

                return task;
            }
            catch(OperationCanceledException ope)
            { }
            finally
            {
                try {
                    OnTaskStopped();
                }
                catch(Exception exc) { }
            }
            return null;
        }

        protected abstract Task Execute(TaskExecutionContext context, ProgressChange onProgressChange);
    }
}