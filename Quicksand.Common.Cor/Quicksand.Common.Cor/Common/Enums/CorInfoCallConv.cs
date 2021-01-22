using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [Flags]
    public enum CorInfoCallConv : Byte
    {
        CorInfoCallConvDefault      = 0x0,
        CorInfoCallConvC            = 0x1,
        CorInfoCallConvStdCall      = 0x2,
        CorInfoCallConvThisCall     = 0x3,
        CorInfoCallConvFastCall     = 0x4,
        CorInfoCallConvVararg       = 0x5,
        CorInfoCallConvField        = 0x6,
        CorInfoCallConvLocalSig     = 0x7,
        CorInfoCallConvProperty     = 0x8,
        CorInfoCallConvNativeVararg = 0xb, 
        CorInfoCallConvMask         = 0x0f,   
        CorInfoCallConvGeneric      = 0x10,
        CorInfoCallConvHasthis      = 0x20,
        CorInfoCallConvExplicitThis = 0x40,
        CorInfoCallConvParamType    = 0x80,
    }
}
