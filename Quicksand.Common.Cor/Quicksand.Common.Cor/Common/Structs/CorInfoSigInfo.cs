using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct CorInfoSigInfo
    {
        internal CorInfoCallConv  CallConv;
        internal IntPtr           RetTypeClass;
        internal IntPtr           RetTypeSigClass;
        internal CorInfoType      RetType;
        internal Byte             Flags;
        internal Int16            NumArgs;
        internal CorInfoSigInst   SigInst;
        internal IntPtr           Args;
        internal IntPtr           PSig;
        internal Int32            CbSig;
        internal IntPtr           Scope;
        internal Int32            Token;
    }
}
