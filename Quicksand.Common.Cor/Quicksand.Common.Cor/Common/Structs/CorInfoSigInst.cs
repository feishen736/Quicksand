using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct CorInfoSigInst
    {
        internal Int32    ClassInstCount;
        internal IntPtr   ClassInst;
        internal Int32    MethInstCount;
        internal IntPtr   MethInst;
    }
}
