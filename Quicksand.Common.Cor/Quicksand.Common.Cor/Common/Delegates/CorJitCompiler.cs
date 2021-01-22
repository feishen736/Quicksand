
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    /// <summary>
    /// 编译方法
    /// </summary>
    /// <param name="thisPtr">当前指针</param>
    /// <param name="corJitInfo">描述指针</param>
    /// <param name="methodInfo">方法信息</param>
    /// <param name="flags">方法标识</param>
    /// <param name="nativeEntry">入口地址</param>
    /// <param name="nativeSizeOfCode">地址大小</param>
    /// <returns></returns>
    [CorInfoOffset("4.5.0000.0", 0, 0)]
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate Int32 CompileMethodDelegate(IntPtr thisPtr, [In] IntPtr corJitInfo, [In] ref CorInfoMethodInfo corMethodInfo, [In] Int32 flags, [Out] IntPtr nativeEntry, [Out] IntPtr nativeSizeOfCode);
}
