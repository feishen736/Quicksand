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
    /// Jit 编译
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal class CorJitCompiler : CorJitHooking
    {
        #region 字段
        private Int32 locked;
        private CorJitDescribe describe;
        #endregion

        #region 委托
        private CompileMethodDelegate compileMethodSource;
        private CompileMethodDelegate compileMethodTarget;
        #endregion

        #region 构造
        internal CorJitCompiler(IntPtr thisPtr) : base(thisPtr)
        {

        }
        #endregion

        #region 方法
        /// <summary>
        /// 替换方法
        /// </summary>
        internal override void ReplaceFunction()
        {
            // 获取委托
            this.compileMethodSource = base.GetDelegate<CompileMethodDelegate>(base.ThisPtr);
            this.compileMethodTarget = this.CompileMethod;
            // 设置委托
            base.SetDelegate<CompileMethodDelegate>(base.ThisPtr, compileMethodTarget);
        }
        /// <summary>
        /// 还原方法
        /// </summary>
        internal override void RestoreFunction()
        {
            // 还原方法
            this.describe.RestoreFunction();
            // 设置委托
            base.SetDelegate<CompileMethodDelegate>(base.ThisPtr, new CompileMethodDelegate(this.compileMethodSource));
        }

        /// <summary>
        /// 编译方法
        /// </summary>
        /// <param name="thisPtr">当前指针</param>
        /// <param name="corJitInfo">描述指针</param>
        /// <param name="corMethodInfo">方法信息</param>
        /// <param name="flags">方法标识</param>
        /// <param name="nativeEntry">入口地址</param>
        /// <param name="nativeSizeOfCode">地址大小</param>
        /// <returns>执行结果</returns>
        internal Int32 CompileMethod(IntPtr thisPtr, IntPtr corJitInfo, ref CorInfoMethodInfo corMethodInfo, Int32 flags, IntPtr nativeEntry, IntPtr nativeSizeOfCode)
        {
            // 延时回收
            GC.KeepAlive(this.compileMethodTarget);

            // 创建对象
            if (this.describe == null)
                this.locked++;
            if (this.locked == 1)
                describe = new CorJitDescribe(corJitInfo);
            if (this.describe == null)
                return this.compileMethodSource(thisPtr, corJitInfo, ref corMethodInfo, flags, nativeEntry, nativeSizeOfCode);

            // 方法信息
            var compilerMethodInfo = new CompilerMethodInfo();
            compilerMethodInfo.MethodDesc = corMethodInfo;
            compilerMethodInfo.MethodInfo = describe.GetMethodInfo(corJitInfo, corMethodInfo);

            // 回调编译
            if (CorJitBehavier.JitCompiler(compilerMethodInfo) == false)
                return this.compileMethodSource(thisPtr, corJitInfo, ref corMethodInfo, flags, nativeEntry, nativeSizeOfCode);

            CorJitBehavier.CompilerMethodInfos.Add(compilerMethodInfo);
            var returnValue = this.compileMethodSource(thisPtr, corJitInfo, ref compilerMethodInfo.MethodDesc, flags, nativeEntry, nativeSizeOfCode);
            CorJitBehavier.CompilerMethodInfos.Remove(compilerMethodInfo);

            // 释放内存
            if (corMethodInfo.ILCode != compilerMethodInfo.MethodDesc.ILCode)
                Marshal.FreeHGlobal(compilerMethodInfo.MethodDesc.ILCode);

            // 返回结果
            return returnValue;
        }
        #endregion
    }
}
