using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsefullAlgorithms.ChainedExecution.API;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace UsefullAlgorithms.ChainedExecution.Tests
{
    [TestClass]
    public class ChainedExecutionTests
    {
        private class OrderCheckTask : SyncTaskBase
        {
            private ConcurrentStack<int> order;

            public OrderCheckTask(ConcurrentStack<int> order)
            {
                this.order = order;
            }

            public override void Run(TaskExecutionContext context, ProgressChange onProgressChange)
            {
                order.Push(context.TaskId);
            }
        }

        private class OrderCheckTaskAsync : AsyncTaskBase
        {
            private ConcurrentStack<int> order;

            public OrderCheckTaskAsync(ConcurrentStack<int> order)
            {
                this.order = order;
            }

            public async override Task Run(TaskExecutionContext context, ProgressChange onProgressChange)
            {
                order.Push(context.TaskId);
                await Task.Delay(0);
            }
        }

        private ConcurrentStack<int> order = new ConcurrentStack<int>();

        [TestMethod]
        public void ChceckClassesExecutedInAppropiateOrder_ShouldPass()
        {
            ExecutionContextBuilder b = new ExecutionContextBuilder();
            b.WithChildTask<OrderCheckTask>(0, new object[] { order }).Return();
            b.WithChildTask<OrderCheckTask>(1, new object[] { order }).Return();
            b.WithChildTask<OrderCheckTask>(2, new object[] { order }, 0, 1).Return();
            b.WithChildTask<OrderCheckTask>(3, new object[] { order }, 2).Return();
            
            var env = b.Build();
            
            env.Execute();

            var p = order.Reverse().ToArray();

            Assert.AreEqual(0, p[0]);
            Assert.AreEqual(1, p[1]);
            Assert.AreEqual(2, p[2]);
            Assert.AreEqual(3, p[3]);
        }

        [TestMethod]
        public void ChceckClassesExecutedAsyncInAppropiateOrder_ShouldPass()
        {
            ExecutionContextBuilder b = new ExecutionContextBuilder();
            b.WithChildTask<OrderCheckTaskAsync>(0, new object[] { order }).Return();
            b.WithChildTask<OrderCheckTaskAsync>(1, new object[] { order }).Return();
            b.WithChildTask<OrderCheckTaskAsync>(2, new object[] { order }, 0, 1).Return();
            b.WithChildTask<OrderCheckTaskAsync>(3, new object[] { order }, 2).Return();

            var env = b.Build();

            env.Execute();

            var p = order.Reverse().ToArray();

            Assert.AreEqual(0, p[0]);
            Assert.AreEqual(1, p[1]);
            Assert.AreEqual(2, p[2]);
            Assert.AreEqual(3, p[3]);
        }

        [TestMethod]
        public void CheckActionsExecutedInAppropiateOrder_ShouldPass()
        {
            ExecutionContextBuilder b = new ExecutionContextBuilder();
            b.WithChildTask(0, (context, onProgressChange) => { order.Push(context.TaskId); }).Return();
            b.WithChildTask(1, (context, onProgressChange) => { order.Push(context.TaskId); }).Return();
            b.WithChildTask(2, (context, onProgressChange) => { order.Push(context.TaskId); }, 0, 1).Return();
            b.WithChildTask(3, (context, onProgressChange) => { order.Push(context.TaskId); }, 2).Return();

            var env = b.Build();

            env.Execute();

            var p = order.Reverse().ToArray();

            Assert.IsTrue(p[0] == 0 || p[0] == 1);
            Assert.IsTrue(p[1] == 0 || p[1] == 1);
            Assert.AreEqual(2, p[2]);
            Assert.AreEqual(3, p[3]);
        }

        [TestMethod]
        public void CheckFunctionsExecutedInAppropiateOrder_ShouldPass()
        {
            ExecutionContextBuilder b = new ExecutionContextBuilder();
            b.WithChildTask(0, async (context, onProgressChange) => { order.Push(context.TaskId); await Task.Delay(0); }).Return();
            b.WithChildTask(1, async (context, onProgressChange) => { order.Push(context.TaskId); await Task.Delay(0); }).Return();
            b.WithChildTask(2, async (context, onProgressChange) => { order.Push(context.TaskId); await Task.Delay(0); }, 0, 1).Return();
            b.WithChildTask(3, async (context, onProgressChange) => { order.Push(context.TaskId); await Task.Delay(0); }, 2).Return();

            var env = b.Build();

            env.Execute();

            var p = order.Reverse().ToArray();

            Assert.IsTrue(p[0] == 0 || p[0] == 1);
            Assert.IsTrue(p[1] == 0 || p[1] == 1);
            Assert.AreEqual(2, p[2]);
            Assert.AreEqual(3, p[3]);
        }

        [TestMethod]
        public void CheckMixedExecutionInAppropiateOrder_ShouldPass()
        {
            ExecutionContextBuilder b = new ExecutionContextBuilder();
            b.WithChildTask<OrderCheckTask>(0, new object[] { order }).Return();
            b.WithChildTask<OrderCheckTaskAsync>(1, new object[] { order }).Return();
            b.WithChildTask(2, (context, onProgressChange) => { order.Push(context.TaskId); }, 0, 1).Return();
            b.WithChildTask(3, async (context, onProgressChange) => { order.Push(context.TaskId); await Task.Delay(0); }, 2).Return();

            var env = b.Build();

            env.Execute();

            var p = order.Reverse().ToArray();

            Assert.IsTrue(p[0] == 0 || p[0] == 1);
            Assert.IsTrue(p[1] == 0 || p[1] == 1);
            Assert.AreEqual(2, p[2]);
            Assert.AreEqual(3, p[3]);
        }

        [TestMethod]
        public void CheckProgressReport()
        {
            int expProgress = 1;
            ExecutionContextBuilder b = new ExecutionContextBuilder();
            b.WithChildTask(0, (context, progress) => {
                progress(1);
                progress(2);
                progress(3);
            }).WithProgressChange((val) => Assert.AreEqual(expProgress++, val)).Return();

            var env = b.Build();

            env.Execute();
        }
    }
}
