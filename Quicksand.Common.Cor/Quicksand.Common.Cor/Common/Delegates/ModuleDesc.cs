using Quicksand.Common.Cor.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common.Delegates
{
    /// <summary>
    /// 获取 IMetaDataEmit 实例
    /// </summary>
    /// <param name="thisPtr">当前指针</param>
    /// <returns>IMetaDataEmit 实例</returns>
    [CorInfoOffset("4.8.3761.0", 925038, 925038)]
    [CorInfoOffset("4.7.3362.0", 1188558, 3249140)]
    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    internal delegate IntPtr GetEmitter(IntPtr thisPtr);
}
