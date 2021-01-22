using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [Flags]
    internal enum CorInfoInline : Int32
    {
        InlinePass  = 0,
        InlineFail  = -1,
        InlineNever = -2,
    }
}
