

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    /// <summary>
    /// 获取方法签名
    /// </summary>
    /// <param name="thisPtr">当前指针</param>
    /// <param name="methodHandle">方法句柄</param>
    /// <param name="corMethodInfo">方法信息</param>
    /// <returns>类型指针</returns>
    [CorInfoOffset("4.5.0000.0", 3, 3)]
    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    internal delegate void GetMethodSigDelegate(IntPtr thisPtr, [In] IntPtr methodHandle, [Out] out CorInfoMethodInfo corMethodInfo, [In] IntPtr classHandle);
    /// <summary>
    /// 能否内联方法
    /// </summary>
    /// <param name="thisPtr">当前指针</param>
    /// <param name="callerMethodHandle1">调用方法句柄</param>
    /// <param name="calleeMethodHandle1">被调方法句柄</param>
    /// <param name="Restrictions">限制</param>
    /// <returns>能否内联方法</returns>
    [CorInfoOffset("4.5.0000.0", 4, 4)]
    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    internal delegate CorInfoInline CanInlineDelegate(IntPtr thisPtr, [In] IntPtr callerMethodHandle1, [In] IntPtr calleeMethodHandle1, [Out] out Int32 Restrictions);
    /// <summary>
    /// 获取子句信息
    /// </summary>
    /// <param name="thisPtr">当前指针</param>
    /// <param name="methodHandle">方法句柄</param>
    /// <param name="clauseNumber">子句号码</param>
    /// <param name="clauseInfo">子句信息</param>
    [CorInfoOffset("4.5.0000.0", 8, 8)]
    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    internal delegate void GetClauseInfoDelegate(IntPtr thisPtr, [In] IntPtr methodHandle, [In] Int32 clauseNumber, [In] ref CorInfoClauseInfo clauseInfo);
    /// <summary>
    /// 获取方法类型
    /// </summary>
    /// <param name="thisPtr">当前指针</param>
    /// <param name="methodHandle">方法句柄</param>
    /// <returns>类型指针</returns>
    //[CorInfoOffset("4.8.4010.0", 9, 9)]
    //[CorInfoOffset("4.8.3761.0", 9, 9)]
    //[CorInfoOffset("4.7.3362.0", 9, 9)]
    //[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    //internal delegate IntPtr GetMethodClassDelegate(IntPtr thisPtr, [In] IntPtr methodHandle);
    /// <summary>
    /// 令牌是否有效
    /// </summary>
    /// <param name="thisPtr">当前指针</param>
    /// <param name="moduleHandle">模块句柄</param>
    /// <param name="metadataToken">元数据令牌</param>
    /// <returns>令牌是否有效</returns>
    //[CorInfoOffset("4.8.4010.0", 34, 34)]
    //[CorInfoOffset("4.8.3761.0", 32, 32)]
    //[CorInfoOffset("4.7.3362.0", 32, 32)]
    //[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    //internal delegate Boolean IsValidTokenDelegate(IntPtr thisPtr, [In] IntPtr moduleHandle, [In] Int32 metadataToken);
    ///// <summary>
    /// 获取类型模块
    /// </summary>
    /// <param name="thisPtr">当前指针</param>
    /// <param name="classHandle">类型句柄</param>
    /// <returns>模块指针</returns>
    //[CorInfoOffset("4.8.4010.0", 46, 46)]
    //[CorInfoOffset("4.8.3761.0", 42, 42)]
    //[CorInfoOffset("4.7.3362.0", 42, 42)]
    //[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    //internal delegate IntPtr GetClassModuleDelegate(IntPtr thisPtr, [In] IntPtr classHandle);
    /// <summary>
    /// 获取模块类库
    /// </summary>
    /// <param name="thisPtr">当前指针</param>
    /// <param name="moduleHandle">模块句柄</param>
    /// <returns>类库句柄</returns>
    //[CorInfoOffset("4.8.4010.0", 47, 47)]
    //[CorInfoOffset("4.8.3761.0", 47, 47)]
    //[CorInfoOffset("4.7.3362.0", 43, 43)]
    //[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    //internal delegate IntPtr GetModuleAssemblyDelegate(IntPtr thisPtr, [In] IntPtr moduleHandle);
    /// <summary>
    /// 获取类库名称
    /// </summary>
    /// <param name="thisPtr">当前指针</param>
    /// <param name="assemblyHandle">类库句柄</param>
    /// <returns>类库名称</returns>
    //[CorInfoOffset("4.8.4010.0", 48, 48)]
    //[CorInfoOffset("4.8.3761.0", 48, 48)]
    //[CorInfoOffset("4.7.3362.0", 44, 44)]
    //[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    //internal delegate IntPtr GetAssemblyNameDelegate(IntPtr thisPtr, [In] IntPtr assemblyHandle);
    /// <summary>
    /// 获取方法令牌
    /// </summary>
    /// <param name="thisPtr">当前指针</param>
    /// <param name="methodHandle">方法句柄</param>
    /// <returns>令牌标识</returns>
    //[CorInfoOffset("4.8.4010.0", 112, 112)]
    //[CorInfoOffset("4.8.3761.0", 112, 112)]
    //[CorInfoOffset("4.7.3362.0", 105, 105)]
    //[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    //internal delegate Int32 GetMethodDefFromMethodDelegate(IntPtr thisPtr, [In] IntPtr methodHandle);

}
