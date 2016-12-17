using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    internal class TaskPoolPolicy
    {
        internal enum AmountToUse : int
        {
            Infinity = -1
        }

        private readonly int allowedTasksInUse = (int)AmountToUse.Infinity;
        private int tasksInUse = 0;

        public TaskPoolPolicy()
            : this((int)AmountToUse.Infinity)
        { }

        public TaskPoolPolicy(int allowedTasksInUse)
        {
            this.allowedTasksInUse = allowedTasksInUse;
        }

        public Task Take(Func<Task> create)
        {
            Interlocked.Increment(ref tasksInUse);
            return create();
        }

        public bool CanCreateTask => allowedTasksInUse == (int)AmountToUse.Infinity || allowedTasksInUse > tasksInUse;
        
        public void TakeBack()
        {
            Interlocked.Decrement(ref tasksInUse);
        }
    }
}
