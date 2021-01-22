using Quicksand.Common.Cor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor
{
    /// <summary>
    /// Jit 支持
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public class CorJitProvider
    {
        #region 方法
        /// <summary>
        /// Jit 编译
        /// </summary>
        /// <param name="compilerMethodInfo">编译方法</param>
        /// <returns>是否编译方法</returns>
        protected virtual Boolean JitCompiler(CompilerMethodInfo compilerMethodInfo)
        {
            return false;
        }

        /// <summary>
        /// Jit 内联
        /// </summary>
        /// <param name="callerMethodInfo">调用方法</param>
        /// <param name="calleeMethodInfo">被调方法</param>
        /// <returns>是否内联函数</returns>
        protected virtual Boolean JitInlining(MethodBase callerMethodInfo, MethodBase calleeMethodInfo)
        {
            return true;
        }
        #endregion
    }
}
