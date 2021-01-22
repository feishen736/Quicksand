using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [Flags]
    internal enum CorInfoRegionKind : Int32
    {
        CorInfoRegingNone   = 0,
        CorInfoRegingHot    = 1,
        CorInfoRegingCold   = 2,
        CorInfoRegingJit    = 3,
    }
}
