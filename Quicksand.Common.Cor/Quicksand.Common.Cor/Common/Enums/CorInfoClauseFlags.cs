using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [Flags]
    internal enum CorInfoClauseFlags : Int32
    {
        CorInfoClauseNone       = 0x0000,
        CorInfoClauseFilter     = 0x0001, 
        CorInfoClauseFinally    = 0x0002, 
        CorInfoClauseFault      = 0x0004,
        CorInfoClauseDuplicate  = 0x0008,
        CorInfoClauseSametry    = 0x0010, 
    }
}
