using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [Flags]
    internal enum CorInfoFlag : UInt32
    {
        CorInfoFlgProtected             = 0x00000004,
        CorInfoFlgStatic                = 0x00000008,
        CorInfoFlgFinal                 = 0x00000010,
        CorInfoFlgSynch                 = 0x00000020,
        CorInfoFlgVirtual               = 0x00000040,
        CorInfoFlgNative                = 0x00000100,
        CorInfoFlgIntrinsicType         = 0x00000200,
        CorInfoFlgAbstract              = 0x00000400,
        CorInfoFlgEnC                   = 0x00000800, 
        CorInfoFlgForceInline           = 0x00010000, 
        CorInfoFlgSharedInst            = 0x00020000,
        CorInfoFlgDelegateInvoke        = 0x00040000, 
        CorInfoFlgPinvoke               = 0x00080000,
        CorInfoFlgSecurityCheck         = 0x00100000,
        CorInfoFlgNogcCheck             = 0x00200000, 
        CorInfoFlgIntrInSic             = 0x00400000,
        CorInfoFlgConstructor           = 0x00800000, 
        CorInfoFlgNoSecurityWrap        = 0x04000000,
        CorInfoFlgDontInline            = 0x10000000,
        CorInfoFlgDontInlineCaller      = 0x20000000, 
        CorInfoFlgJitIntrinsic          = 0x40000000, 
        CorInfoFlgValueClass            = 0x00010000, 
        CorInfoFlgVarObjSize            = 0x00040000,
        CorInfoFlgArray                 = 0x00080000, 
        CorInfoFlgOverlappingFields     = 0x00100000, 
        CorInfoFlgInterface             = 0x00200000,
        CorInfoFlgContextFul            = 0x00400000, 
        CorInfoFlgCustomLayout          = 0x00800000, 
        CorInfoFlgContainsGcPtr         = 0x01000000, 
        CorInfoFlgDelegate              = 0x02000000,
        CorInfoFlgMarshalByRef          = 0x04000000, 
        CorInfoFlgContainsStackPtr      = 0x08000000, 
        CorInfoFlgVariance              = 0x10000000, 
        CorInfoFlgBeforeFieldInit       = 0x20000000, 
        CorInfoFlgGenericTypeVariable   = 0x40000000,
        CorInfoFlgUnsafeValueClass      = 0x80000000, 
    }
}
