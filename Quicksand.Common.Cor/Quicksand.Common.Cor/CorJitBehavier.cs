using Quicksand.Common.Cor.Common;
using System;
using System.Collections.Concurrent;
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
    /// Jit 行为
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal static class CorJitBehavier
    {
        #region 属性
        /// <summary>
        /// 方法信息
        /// </summary>
        internal static List<CompilerMethodInfo> CompilerMethodInfos;
        #endregion

        #region 委托
        /// <summary>
        /// Jit 编译
        /// </summary>
        internal static JitCompilerDelegate JitCompiler = null;
        /// <summary>
        /// Jit 内联
        /// </summary>
        internal static JitInliningDelegate JitInlining = null;
        #endregion

        #region 构造
        static CorJitBehavier()
        {
            CorJitBehavier.CompilerMethodInfos = new List<CompilerMethodInfo>();
        }
        #endregion
    }
}
