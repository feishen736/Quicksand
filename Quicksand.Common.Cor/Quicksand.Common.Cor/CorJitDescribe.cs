

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
    /// Jit 描述
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal class CorJitDescribe : CorJitHooking
    {
        #region 委托
        private CanInlineDelegate canInlineSource;
        private GetMethodSigDelegate getMethodSigSource;
        private GetClauseInfoDelegate getClauseInfoSource;

        private CanInlineDelegate canInlineTarget;
        private GetClauseInfoDelegate getClauseInfoTarget;
        #endregion

        #region 构造
        internal CorJitDescribe(IntPtr thisPtr) : base(thisPtr)
        {

        }
        #endregion

        #region 方法
        /// <summary>
        /// 替换函数
        /// </summary>
        internal override void ReplaceFunction()
        {
            // 获取委托
            this.canInlineSource = base.GetDelegate<CanInlineDelegate>(base.ThisPtr);
            this.getMethodSigSource = base.GetDelegate<GetMethodSigDelegate>(base.ThisPtr);
            this.getClauseInfoSource = base.GetDelegate<GetClauseInfoDelegate>(base.ThisPtr);
            // 设置委托
            this.canInlineTarget = new CanInlineDelegate(this.CanInline);
            this.getClauseInfoTarget = new GetClauseInfoDelegate(this.GetClauseInfo);

            base.SetDelegate<CanInlineDelegate>(base.ThisPtr, this.canInlineTarget);
            base.SetDelegate<GetClauseInfoDelegate>(base.ThisPtr, this.getClauseInfoTarget);
        }
        /// <summary>
        /// 恢复函数
        /// </summary>
        internal override void RestoreFunction()
        {
            // 设置委托
            base.SetDelegate<CanInlineDelegate>(base.ThisPtr, new CanInlineDelegate(this.canInlineSource));
            base.SetDelegate<GetClauseInfoDelegate>(base.ThisPtr, new GetClauseInfoDelegate(this.getClauseInfoSource));
        }

        /// <summary>
        /// 获取方法描述
        /// </summary>
        /// <param name="thisPtr">当前指针</param>
        /// <param name="corMehtodInfo">方法信息</param>
        /// <returns>方法信息</returns>
        internal MethodBase GetMethodInfo(IntPtr thisPtr, CorInfoMethodInfo corMehtodInfo)
        {
            if (corMehtodInfo.MethodHandle == IntPtr.Zero)
                return null;
            if (corMehtodInfo.ModuleHandle == IntPtr.Zero)
                this.getMethodSigSource(thisPtr, corMehtodInfo.MethodHandle, out corMehtodInfo, IntPtr.Zero);

            var methodHandle = corMehtodInfo.MethodHandle;
            var moduleHandle = corMehtodInfo.ModuleHandle;

            var methodToken = Convert.ToInt32(SigTokenType.MethodDef) + Marshal.ReadByte(methodHandle);

            foreach (var assemblya in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var module in assemblya.GetModules())
                {
                    var moduleHandleData = this.GetType().Module.GetType().GetField("m_pData", BindingFlags.NonPublic | BindingFlags.Instance);
                    var moduleHandleValue = (IntPtr)moduleHandleData.GetValue(module);

                    if (moduleHandleValue != corMehtodInfo.ModuleHandle)
                        continue;
                    if (!module.IsValidMetadataToken(methodToken))
                        continue;

                    return module.ResolveMethod(methodToken);
                }
            }

            return null;
        }
        /// <summary>
        /// 获取方法标识
        /// </summary>
        /// <param name="thisPtr">当前指针</param>
        /// <param name="methodHandle1">方法句柄</param>
        /// <returns>方法标识</returns>
        internal CorInfoInline CanInline(IntPtr thisPtr, IntPtr callerMethodHandle1, IntPtr calleeMethodHandle1, out Int32 Restrictions)
        {
            // 延时回收
            GC.KeepAlive(canInlineTarget);

            var callerMethodInfo = this.GetMethodInfo(thisPtr, new CorInfoMethodInfo() { MethodHandle = callerMethodHandle1 });
            var calleeMethodInfo = this.GetMethodInfo(thisPtr, new CorInfoMethodInfo() { MethodHandle = calleeMethodHandle1 });

            var canInline = false;
            var inlineInfo = this.canInlineSource(thisPtr, callerMethodHandle1, calleeMethodHandle1, out Restrictions);

            if (callerMethodInfo == null || calleeMethodInfo == null)
                return inlineInfo;
            else
                canInline = CorJitBehavier.JitInlining(callerMethodInfo, calleeMethodInfo);

            if (canInline)
                return inlineInfo;
            else
                return CorInfoInline.InlineNever;
        }
        /// <summary>
        /// 获取子句信息
        /// </summary>
        /// <param name="thisPtr">当前指针</param>
        /// <param name="methodHandle">方法句柄</param>
        /// <param name="clauseNumber">子句号码</param>
        /// <param name="clauseInfo">子句信息</param>
        internal void GetClauseInfo(IntPtr thisPtr, IntPtr methodHandle, Int32 clauseNumber, ref CorInfoClauseInfo clauseInfo)
        {
            // 延时回收
            GC.KeepAlive(getClauseInfoTarget);

            CompilerMethodInfo compilerMethodInfo = null;

            foreach (var item in CorJitBehavier.CompilerMethodInfos)
                if (item == null)
                    continue;
                else if (item.MethodDesc.MethodHandle == methodHandle)
                    compilerMethodInfo = item;

            if (compilerMethodInfo == null)
                this.getClauseInfoSource(thisPtr, methodHandle, clauseNumber, ref clauseInfo);
            else if (compilerMethodInfo.ClauseInfo.Count == 0)
                this.getClauseInfoSource(thisPtr, methodHandle, clauseNumber, ref clauseInfo);
            else
                clauseInfo = compilerMethodInfo.ClauseInfo[clauseNumber];
        }
        #endregion
    }
}
