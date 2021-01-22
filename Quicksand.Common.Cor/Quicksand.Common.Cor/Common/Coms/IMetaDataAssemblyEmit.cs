using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [ComImport]
    [Guid("211EF15B-5317-4438-B196-DEC87B887693")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal interface IMetaDataAssemblyEmit
    {

    }
}
