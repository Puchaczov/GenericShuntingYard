using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution.API
{
    public class ExecutionContextBuilder
    {
        public class TaskEventBuilder
        {
            event TaskExecutionStarted taskExecutionStarted;
            event TaskExecutionStopped taskExecutionStopped;
            event ProgressChange progressChange;

            private ExecutionContextBuilder builder;
            private Dictionary<int, ExecutionTaskInfo> tasks;
            private Dictionary<int, int[]> deps;
            private int id;
            private Func<TaskExecutionStarted, TaskExecutionStopped, ProgressChange, ExecutionTaskInfo> p;

            internal TaskEventBuilder(ExecutionContextBuilder builder, Dictionary<int, ExecutionTaskInfo> tasks, Dictionary<int, int[]> deps)
            {
                this.tasks = tasks;
                this.deps = deps;
                this.builder = builder;
                this.taskExecutionStarted = new TaskExecutionStarted(() => { });
                this.taskExecutionStopped = new TaskExecutionStopped(() => { });
                this.progressChange = new ProgressChange((val) => { });
            }

            internal TaskEventBuilder(ExecutionContextBuilder builder, Dictionary<int, ExecutionTaskInfo> tasks, Dictionary<int, int[]> deps, int id) : this(builder, tasks, deps)
            {
                this.id = id;
            }

            internal TaskEventBuilder(ExecutionContextBuilder builder, Dictionary<int, ExecutionTaskInfo> tasks, Dictionary<int, int[]> deps, int id, Func<TaskExecutionStarted, TaskExecutionStopped, ProgressChange, ExecutionTaskInfo> p) : this(builder, tasks, deps, id)
            {
                this.p = p;
            }

            public TaskEventBuilder WithEventOnStart(TaskExecutionStarted startedDelegate)
            {
                taskExecutionStarted = new TaskExecutionStarted(startedDelegate);
                return this;
            }

            public TaskEventBuilder WithEventOnStop(TaskExecutionStopped stoppedDelegate)
            {
                taskExecutionStopped = new TaskExecutionStopped(stoppedDelegate);
                return this;
            }

            public TaskEventBuilder WithProgressChange(ProgressChange change)
            {
                progressChange += change;
                return this;
            }

            public ExecutionContextBuilder Return()
            {
                tasks.Add(id, this.p(taskExecutionStarted, taskExecutionStopped, progressChange));
                return builder;
            }
        }

        private Dictionary<int, ExecutionTaskInfo> tasks;
        private Dictionary<int, int[]> deps;
        private int allowedThreadsToRun;

        public ExecutionContextBuilder()
        {
            tasks = new Dictionary<int, ExecutionTaskInfo>();
            deps = new Dictionary<int, int[]>();
            allowedThreadsToRun = (int)TaskPoolPolicy.AmountToUse.Infinity;
        }

        public ExecutionContextBuilder WithChildTask<T>(int id, TaskExecutionStarted delegateStareted, TaskExecutionStopped delegateStopped, ProgressChange delegateProgressChange, params int[] dependencies) where T : ChainedTaskBase => WithChildTask<T>(id, new object[0], delegateStareted, delegateStopped, delegateProgressChange, dependencies);

        public TaskEventBuilder WithChildTask<T>(int id, params int[] dependencies)
        {
            deps.Add(id, dependencies);
            return new TaskEventBuilder(this, tasks, deps, id, (tstarted, tstopped, onProgressChange) => new ClassTaskExecutor(typeof(T), new object[0], id, tstarted, tstopped, onProgressChange));
        }

        public ExecutionContextBuilder WithChildTask<T>(int id, object[] args, TaskExecutionStarted delegateStareted, TaskExecutionStopped delegateStopped, ProgressChange delegateProgressChange, params int[] dependencies) where T : ChainedTaskBase
        {
            tasks.Add(id, new ClassTaskExecutor(typeof(T), args, id, delegateStareted, delegateStopped, delegateProgressChange));
            deps.Add(id, dependencies);
            return this;
        }

        public TaskEventBuilder WithChildTask<T>(int id, object[] args, params int[] dependencies)
        {
            this.deps.Add(id, dependencies);
            return new TaskEventBuilder(this, tasks, deps, id, (tstarted, tstopped, onProgressChange) => new ClassTaskExecutor(typeof(T), args, id, tstarted, tstopped, onProgressChange));
        }

        public ExecutionContextBuilder WithChildTask(int id, Func<TaskExecutionContext, ProgressChange, Task> func, TaskExecutionStarted delegateStareted, TaskExecutionStopped delegateStopped, ProgressChange delegateProgressChange, params int[] dependencies)
        {
            tasks.Add(id, new FuncExecutionTaskInfo(func, id, delegateStareted, delegateStopped, delegateProgressChange));
            deps.Add(id, dependencies);
            return this;
        }

        public TaskEventBuilder WithChildTask(int id, Func<TaskExecutionContext, ProgressChange, Task> func, params int[] depenedencies)
        {
            deps.Add(id, depenedencies);
            return new TaskEventBuilder(this, tasks, deps, id, (TaskExecutionStarted tstarted, TaskExecutionStopped tstopped, ProgressChange delegateProgressChange) => new FuncExecutionTaskInfo(func, id, tstarted, tstopped, delegateProgressChange));
        }

        public ExecutionContextBuilder WithChildTask(int id, Action<TaskExecutionContext, ProgressChange> action, TaskExecutionStarted delegateStareted, TaskExecutionStopped delegateStopped, ProgressChange delegateProgressChange, params int[] dependencies)
        {
            tasks.Add(id, new ActionExecutionTaskInfo(action, id, delegateStareted, delegateStopped, delegateProgressChange));
            deps.Add(id, dependencies);
            return this;
        }

        public TaskEventBuilder WithChildTask(int id, Action<TaskExecutionContext, ProgressChange> action, params int[] dependencies)
        {
            deps.Add(id, dependencies);
            return new TaskEventBuilder(this, tasks, deps, id, (TaskExecutionStarted tstarted, TaskExecutionStopped tstopped, ProgressChange delegateProgressChange) => new ActionExecutionTaskInfo(action, id, tstarted, tstopped, delegateProgressChange));
        }

        public ExecutionContextBuilder WithConcurrencyLimit(int allowedThreads)
        {
            allowedThreadsToRun = allowedThreads;
            return this;
        }

        public IExecutionEnvironment Build() => new ExecutionEngine(BuildTree(), new TaskPoolPolicy(allowedThreadsToRun));

        private ChainedTaskTree BuildTree()
        {
            ChainedTaskTree tree = new ChainedTaskTree();

            var root = new AlwaysEndedTaskInfo();
            tree.SetRoot(root);
            tree.Add(root);

            foreach(var task in tasks)
            {
                tree.Add(task.Value);

                var depIds = deps[task.Key];
                if(depIds.Length > 0)
                {
                    foreach (var parId in depIds)
                    {
                        tree.Add(tasks[parId]);
                        tree.Connect(tasks[parId], task.Value);
                    }
                }
                else
                {
                    tree.Connect(root, task.Value);
                }
            }
            return tree;
        }
    }
}
