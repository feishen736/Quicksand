using Quicksand.Common.Cor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor
{
    /// <summary>
    /// Jit 挂钩
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal abstract class CorJitHooking
    {       
        #region 属性
        /// <summary>
        /// This 指针
        /// </summary>
        protected IntPtr ThisPtr { get; private set; }
        #endregion

        #region 构造
        protected CorJitHooking(IntPtr thisPtr)
        {
            this.ThisPtr = thisPtr;
            this.ReplaceFunction();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取委托
        /// </summary>
        /// <typeparam name="T">委托类型</typeparam>
        /// <param name="tablePtr">虚表指针</param>
        /// <returns>委托结果</returns>
        protected T GetDelegate<T>(IntPtr virtualTablePtr, UInt32 protectValue = 0) where T : Delegate
        {
            // 偏移位置
            var ofSet = IntPtr.Size * CorInfoOffsetAttribute.GetCorInfoOffset<T>().Offset;
            // 虚表指针
            var vtPtr = Marshal.ReadIntPtr(virtualTablePtr);
            // 函数指针
            var fcPtr = Marshal.ReadIntPtr(vtPtr, ofSet);

            // 修改保护
            Kernel32.VirtualProtect(fcPtr, (UInt32)IntPtr.Size, Protection.PageExecuteReadWrite, out protectValue);
            // 函数委托
            var fcDlg = Marshal.GetDelegateForFunctionPointer(fcPtr, typeof(T)) as T;
            // 还原保护
            Kernel32.VirtualProtect(fcPtr, (UInt32)IntPtr.Size, (Protection)protectValue, out protectValue);
            // 编译委托
            RuntimeHelpers.PrepareDelegate(fcDlg);

            return fcDlg;
        }
        /// <summary>
        /// 设置委托
        /// </summary>
        /// <typeparam name="T">委托类型</typeparam>
        /// <param name="tablePtr">虚表指针</param>
        /// <param name="delegateMethod">委托方法</param>
        /// <param name="protectValue">保护枚举</param>
        protected void SetDelegate<T>(IntPtr virtualTablePtr, T delegateMethod, UInt32 protectValue = 0) where T : Delegate
        {
            // 偏移位置
            var offsetPtr = IntPtr.Size * CorInfoOffsetAttribute.GetCorInfoOffset<T>().Offset;
            // 资源指针
            var sourcePtr = Marshal.ReadIntPtr(virtualTablePtr) + offsetPtr;
            // 目标指针
            var targetPtr = Marshal.GetFunctionPointerForDelegate(delegateMethod);

            // 编译方法
            RuntimeHelpers.PrepareMethod(delegateMethod.Method.MethodHandle);
            // 修改保护
            Kernel32.VirtualProtect(sourcePtr, (UInt32)IntPtr.Size, Protection.PageExecuteReadWrite, out protectValue);
            // 写入内存
            Marshal.WriteIntPtr(sourcePtr, targetPtr);
            // 还原保护
            Kernel32.VirtualProtect(sourcePtr, (UInt32)IntPtr.Size, (Protection)protectValue, out protectValue);
        }

        /// <summary>
        /// 替换函数
        /// </summary>
        internal abstract void ReplaceFunction();
        /// <summary>
        /// 恢复函数
        /// </summary>
        internal abstract void RestoreFunction();
        #endregion
    }
}
