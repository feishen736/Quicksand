using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct CorInfoClauseInfo
    {
        internal CorInfoClauseFlags Flags;
        internal Int32 TryOffset;
        internal Int32 TryLength;
        internal Int32 HandlerOffset;
        internal Int32 HandlerLength;
        internal Int32 TokenOrOffset;
    }
}
