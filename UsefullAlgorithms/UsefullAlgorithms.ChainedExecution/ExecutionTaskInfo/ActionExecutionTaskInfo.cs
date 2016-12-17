using System;

namespace UsefullAlgorithms.ChainedExecution
{
    internal class ActionExecutionTaskInfo : ExecutionTaskInfo
    {
        private Action<TaskExecutionContext, ProgressChange> action;

        public ActionExecutionTaskInfo(Action<TaskExecutionContext, ProgressChange> action, int id, TaskExecutionStarted delegateStarted, TaskExecutionStopped delegateStopped, ProgressChange delegateProgressChange, ChainedTaskState state = ChainedTaskState.WaitingToBeQueued)
            : base(id, delegateStarted, delegateStopped, delegateProgressChange, state)
        {
            this.action = action;
        }

        public override InternalTaskExecutor Create() => new ChainedTaskAction(action, delegateStarted, delegateStopped, delegateProgressChange);
    }
}
