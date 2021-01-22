using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    /// <summary>
    /// MsCorWks.Dll 导出类
    /// </summary>
    internal static class MsCorWks
    {
        #region 方法
        /// <summary>
        /// 内部查找
        /// </summary>
        /// <param name="internalPtr">内部指针</param>
        /// <param name="riidArray">接口编号</param>
        /// <param name="metadataPtr">外部指针</param>
        /// <returns></returns>
        [DllImport("mscorwks.dll", PreserveSig = true)]
        internal static extern Int32 GetMetaDataPublicInterfaceFromInternal([In] IntPtr internalPtr, [In] Byte[] riidArray, [Out] out IntPtr pulicPtr);

        /// <summary>
        /// 外部查找
        /// </summary>
        /// <param name="pulicPtr">外部指针</param>
        /// <param name="riidArray">接口编号</param>
        /// <param name="internalPtr">内部指针</param>
        /// <returns></returns>
        [DllImport("mscorwks.dll", PreserveSig = true)]
        internal static extern Int32 GetMetaDataInternalInterfaceFromPublic([In] IntPtr pulicPtr, [In] Byte[] riidArray, [Out] out IntPtr internalPtr);
        #endregion
    }
}
