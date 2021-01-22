

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct CorInfoMethodInfo
    {
        internal IntPtr            MethodHandle;
        internal IntPtr            ModuleHandle;
        internal IntPtr            ILCode;
        internal Int32             ILSize;
        internal Int32             MaxStack;
        internal Int32             EHcount;
        internal CorInfoOptions    Options;
        internal CorInfoRegionKind RegionKind;
        internal CorInfoSigInfo    Args;
        internal CorInfoSigInfo    Locals;
    }
}
