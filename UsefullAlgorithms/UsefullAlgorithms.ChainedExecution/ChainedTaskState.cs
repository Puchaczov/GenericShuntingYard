using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.ChainedExecution
{
    public enum ChainedTaskState : Int16
    {
        WaitingToBeQueued,
        Queued,
        Runned,
        Ended,
        Cancelled
    }
}
