using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Aop.Common
{
    [Flags]
    public enum ExecutionStatus
    {
        ExecutionFailed     = 1,

        EntryMethodSuccess  = 2,

        ErrorMethodSuccess  = 3,

        ExitsMethodSuccess  = 4,
    }
}
