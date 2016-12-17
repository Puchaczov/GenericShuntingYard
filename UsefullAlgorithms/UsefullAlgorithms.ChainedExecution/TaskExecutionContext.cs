using System;
using System.Threading;

namespace UsefullAlgorithms.ChainedExecution
{

    public class TaskExecutionContext
    {
        public DateTimeOffset ProcessingStartTime { get; }
        public TimeSpan ElapsedFromStartTime { get; }
        public int TaskId { get; }
        public int ProcessingOrder { get; }
        public CancellationToken CancellationToken { get; }

        internal TaskExecutionContext(DateTimeOffset startTime, TimeSpan elapsed, int taskId, int processingOrder, CancellationToken token)
        {
            this.ProcessingStartTime = startTime;
            this.ElapsedFromStartTime = elapsed;
            this.TaskId = taskId;
            this.ProcessingOrder = processingOrder;
            this.CancellationToken = token;
        }
    }
}