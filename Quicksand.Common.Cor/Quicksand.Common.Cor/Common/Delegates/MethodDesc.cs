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
    /// 获取令牌
    /// </summary>
    /// <param name="thisPtr">当前指针</param>
    /// <returns>令牌指针</returns>
    [CorInfoOffset("4.8.3761.0", 925038, 925038)]
    [CorInfoOffset("4.7.3362.0", 1188556, 1188556)]
    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    internal delegate IntPtr GetTooken(IntPtr thisPtr);
}
