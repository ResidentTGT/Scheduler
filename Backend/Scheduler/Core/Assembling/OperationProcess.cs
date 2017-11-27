using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.Assembling
{
    public enum OperationState
    {
        NotStarted = 0,
        InProcess = 1,
        Finished = 2
    }

    public class OperationProcess
    {
        public Operation Operation { get; set; }

        public OperationState State { get; set; }

        public int ServedCount { get; set; }
    }
}
