using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [Flags]
    internal enum CorInfoType : Byte
    {
        CorInfoTypeUndef        = 0x0,
        CorInfoTypeVoid         = 0x1,
        CorInfoTypeBool         = 0x2,
        CorInfoTypeChar         = 0x3,
        CorInfoTypeByte         = 0x4,
        CorInfoTypeUByte        = 0x5,
        CorInfoTypeShort        = 0x6,
        CorInfoTypeUShort       = 0x7,
        CorInfoTypeInt          = 0x8,
        CorInfoTypeUInt         = 0x9,
        CorInfoTypeLong         = 0xa,
        CorInfoTypeULong        = 0xb,
        CorInfoTypeNativeInt    = 0xc,
        CorInfoTypeNativeUInt   = 0xd,
        CorInfoTypeFloat        = 0xe,
        CorInfoTypeDouble       = 0xf,
        CorInfoTypeString       = 0x10,         
        CorInfoTypePtr          = 0x11,
        CorInfoTypeByRef        = 0x12,
        CorInfoTypeValueClass   = 0x13,
        CorInfoTypeClass        = 0x14,
        CorInfoTypeRefany       = 0x15,
        CorInfoTypeVar          = 0x16,
        CorInfoTypeCount,
    }
}
