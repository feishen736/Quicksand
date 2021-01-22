using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [Flags]
    internal enum CorInfoOptions : Int32
    {
        CorInfoOptInitLocals                = 0x00000010,
        CorInfoGenericsCtxtFromThis         = 0x00000020, 
        CorInfoGenericsCtxtFromMethodDesc   = 0x00000040, 
        CorInfoGenericsCtxtFromMethodTable  = 0x00000080,
        CorInfoGenericsCtxtKeepAlive        = 0x00000100,
        CorInfoGenericsCtxtMask             = CorInfoGenericsCtxtFromThis | CorInfoGenericsCtxtFromMethodDesc |  CorInfoGenericsCtxtFromMethodTable,
    }
}
