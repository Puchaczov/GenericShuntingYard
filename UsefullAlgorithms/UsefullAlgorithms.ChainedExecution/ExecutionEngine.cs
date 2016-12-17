using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    internal class ExecutionEngine : IExecutionEnvironment
    {
        private ChainedTaskTree tree;
        private TaskPoolPolicy taskPoolPolicy;

        public ExecutionEngine(ChainedTaskTree tree, TaskPoolPolicy taskPool)
        {
            this.tree = tree;
            this.taskPoolPolicy = taskPool;
        }

        public void Execute()
        {
            List<Task> awaitables = new List<Task>();

            int performedTasks = 0;
            int processingOrder = 0;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            var now = DateTimeOffset.UtcNow;

            var ts = new CancellationTokenSource();
            while (tree.CountOfWaitingToBeQueued > 0)
            {
                foreach(var taskInfo in tree)
                {
                    if(taskPoolPolicy.CanCreateTask)
                    {
                        processingOrder += 1;
                        var task = taskInfo.Create();
                        var awaitable = taskPoolPolicy.Take(() => task.Execute(
                            new TaskExecutionContext(
                                now, 
                                watch.Elapsed, 
                                taskInfo.Id, 
                                processingOrder, 
                                ts.Token
                            )));

                        if(awaitable == null)
                        {
                            taskPoolPolicy.TakeBack();
                            taskInfo.State = ChainedTaskState.Cancelled;
                            continue;
                        }

                        awaitables.Add(awaitable);
                        awaitables.Add(awaitable.ContinueWith(f =>
                        {
                            Interlocked.Increment(ref performedTasks);
                            taskInfo.State = ChainedTaskState.Ended;
                            taskPoolPolicy.TakeBack();
                        }));
                        taskInfo.State = ChainedTaskState.Queued;
                    }
                    else
                        Task.Delay(100).Wait();
                }
                Task.Delay(100).Wait();
            }
            watch.Stop();

            Task.WaitAll(awaitables.ToArray());
        }
    }
}
