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
    /// Jit 管理
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public static class CorJitManager
    {
        #region 字段
        // Jit 挂钩
        private static CorJitHooking hooking;
        #endregion

        #region 构造
        static CorJitManager()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var constructor = type.GetConstructor(Type.EmptyTypes);

                if (constructor != null && !constructor.IsAbstract && !constructor.ContainsGenericParameters)
                    RuntimeHelpers.PrepareMethod(constructor.MethodHandle);

                foreach (var method in type.GetRuntimeMethods())
                {
                    if (method.IsAbstract || method.ContainsGenericParameters)
                        continue;
                    else
                        RuntimeHelpers.PrepareMethod(method.MethodHandle);
                }
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 附加 Jit 编译
        /// </summary>
        /// <param name="provider">Jit 支持</param>
        public static void AttachJitCompiler(params CorJitProvider[] providers)
        {
            if (providers == null)
                throw new Exception();
            if (providers.Length == 0)
                throw new Exception();
            if (CorJitManager.hooking != null)
                throw new Exception();

            foreach (var provider in providers)
            {
                foreach (var type in provider.GetType().Assembly.GetTypes())
                {
                    foreach (var method in type.GetRuntimeMethods())
                    {
                        if (method.IsAbstract || method.ContainsGenericParameters)
                            continue;
                        else
                            RuntimeHelpers.PrepareMethod(method.MethodHandle);
                    }
                }

                foreach (var method in typeof(CorJitProvider).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    foreach (var field in typeof(CorJitBehavier).GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        if (method.IsVirtual == false)
                            continue;
                        if (method.Name != field.Name)
                            continue;

                        var oldValue = field.GetValue(null) as Delegate;
                        var newValue = Delegate.CreateDelegate(field.FieldType, provider, method);
   
                        field.SetValue(null, newValue);
                    }
                }
            }

            CorJitManager.hooking = new CorJitCompiler(Clrjit.GetJit());
        }

        /// <summary>
        /// 分离 Jit 编译
        /// </summary>
        public static void DetachJitCompiler()
        {
            CorJitManager.hooking.RestoreFunction();
            CorJitManager.hooking = null;
        }
        #endregion
    }
}
