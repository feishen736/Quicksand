using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    /// <summary>
    /// Jit 编译
    /// </summary>
    /// <param name="compilerMethodInfo">编译方法</param>
    /// <returns>是否编译方法</returns>
    internal delegate Boolean JitCompilerDelegate(CompilerMethodInfo compilerMethodInfo);

    /// <summary>
    /// Jit 内联
    /// </summary>
    /// <param name="callerMethodInfo">调用方法</param>
    /// <param name="calleeMethodInfo">被调方法</param>
    /// <returns>是否内联函数</returns>
    internal delegate Boolean JitInliningDelegate(MethodBase callerMethodInfo, MethodBase calleeMethodInfo);
}
