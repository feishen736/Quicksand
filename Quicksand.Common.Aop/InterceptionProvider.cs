using Quicksand.Common.Aop.Common;
using Quicksand.Common.Cor;
using Quicksand.Common.Cor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Aop
{
    /// <summary>
    /// 拦截器支持
    /// </summary>
    public sealed class InterceptionProvider : CorJitProvider
    {
        #region 模板
        /// <summary>
        /// args.Arguments = new List<Object>();
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        private void Template1(CompilerILGenerator generator, CompilerVariableInfo local0, CompilerVariableInfo local1)
        {
            if (generator.GetArgumentInfo().Count == 0)
                return;

            generator.Emit(OpCodes.Ldloc, local1);
            generator.Emit(OpCodes.Newobj, typeof(List<Object>).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Callvirt, typeof(ExecutionArgs).GetProperty("Arguments").SetMethod);
        }
        /// <summary>
        /// args.Arguments.Add(param);
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="thisArg"></param>
        private void Template2(CompilerILGenerator generator, CompilerVariableInfo local0, CompilerVariableInfo local1, CompilerArgumentInfo thisArg)
        {
            foreach (var param in generator.GetArgumentInfo())
            {
                if (thisArg != null && param.ArgumentIndex == 0)
                    continue;

                generator.Emit(OpCodes.Ldloc, local1);
                generator.Emit(OpCodes.Callvirt, typeof(ExecutionArgs).GetProperty("Arguments").GetMethod);
                generator.Emit(OpCodes.Ldarg, param);

                var argsType = param.ArgumentType;
                var typeCode = Type.GetTypeCode(argsType);

                if (argsType.IsByRef)
                {
                    argsType = argsType.GetElementType();
                    typeCode = Type.GetTypeCode(argsType);

                    if (argsType.IsClass)
                        generator.Emit(generator.GetLdind(typeCode));
                    else
                        generator.Emit(OpCodes.Ldobj, argsType);
                }
                if (argsType.IsValueType)
                {
                    generator.Emit(OpCodes.Box, argsType);
                }

                generator.Emit(OpCodes.Callvirt, typeof(List<Object>).GetMethod("Add"));
            }
        }
        /// <summary>
        /// attr.On-XXX-Method(args);
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="methodInfo"></param>
        private void Template3(CompilerILGenerator generator, CompilerVariableInfo local0, CompilerVariableInfo local1, MethodInfo methodInfo)
        {
            generator.Emit(OpCodes.Ldloc, local0);
            generator.Emit(OpCodes.Ldloc, local1);
            generator.Emit(OpCodes.Callvirt, methodInfo);
        }
        /// <summary>
        /// var status = args.Status;
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        private void Template4(CompilerILGenerator generator, CompilerVariableInfo local1, CompilerVariableInfo local2)
        {
            generator.Emit(OpCodes.Ldloc, local1);
            generator.Emit(OpCodes.Callvirt, typeof(ExecutionArgs).GetProperty("Status").GetMethod);
            generator.Emit(OpCodes.Stloc, local2);
        }
        /// <summary>
        /// if (status.HasFlag(ExecutionStatus.-XXX-))
        ///    return;
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        private void Template5(CompilerILGenerator generator, CompilerVariableInfo local2, ExecutionStatus status, List<CompilerLabelInfo> leaves, Boolean trueOrFalse)
        {
            generator.Emit(OpCodes.Ldloc, local2);
            generator.Emit(OpCodes.Box, typeof(ExecutionStatus));
            generator.Emit(OpCodes.Ldc_I4, status);
            generator.Emit(OpCodes.Box, typeof(ExecutionStatus));
            generator.Emit(OpCodes.Call, typeof(Enum).GetMethod("HasFlag"));

            var label = generator.DefineLabel();
            leaves.Add(generator.DefineLabel());

            if (trueOrFalse)
                generator.Emit(OpCodes.Brfalse, label);
            else
                generator.Emit(OpCodes.Brtrue, label);
        }
        #endregion

        #region 方法
        /// <summary>
        /// Jit 内联
        /// </summary>
        /// <param name="callerMethodInfo">调用方法</param>
        /// <param name="calleeMethodInfo">被调方法</param>
        /// <returns>是否内联函数</returns>
        protected override Boolean JitInlining(MethodBase callerMethodInfo, MethodBase calleeMethodInfo)
        {
            return calleeMethodInfo.GetInterceptionAttributes().Count == 0;
        }
        /// <summary>
        /// Jit 编译
        /// </summary>
        /// <param name="compilerMethodInfo">编译方法</param>
        /// <returns>是否编译方法</returns>
        protected override Boolean JitCompiler(CompilerMethodInfo compilerMethodInfo)
        {
            // 获取拦截特性
            var methodInfo = compilerMethodInfo.GetMethodInfo();

            if (methodInfo == null)
                return false;

            var methodAttr = methodInfo.GetInterceptionAttributes();

            if (methodAttr.Count == 0)
                return false;

            // 获取方法生成
            var generator = compilerMethodInfo.GetILGenerator();

            // 获取变量信息
            CompilerArgumentInfo thisArg = null;
            CompilerVariableInfo returnVar = null;
            CompilerVariableInfo exceptionVar = null;
            {
                // 当前对象信息
                if (!methodInfo.Attributes.HasFlag(MethodAttributes.Static))
                {
                    thisArg = generator.DefineArgument(methodInfo.ReflectedType);
                }
                // 参数变量信息
                foreach (var item in methodInfo.GetParameters())
                {
                    generator.DefineArgument(item.ParameterType);
                }

                // 本地变量信息
                foreach (var item in methodInfo.GetMethodBody().LocalVariables)
                {
                    generator.DefineVariable(item.LocalType);
                }
                // 返回结果信息
                if (methodInfo.MemberType == MemberTypes.Method)
                {
                    var returnType = (methodInfo as MethodInfo).ReturnType;

                    if (returnType != typeof(void))
                        returnVar = generator.DefineVariable(returnType);
                }

                // 拦截异常信息
                if (exceptionVar == null)
                {
                    exceptionVar = generator.DefineVariable(typeof(Exception));
                }
            }

            // 生成方法主体
            var variables = new List<Tuple<CompilerVariableInfo, CompilerVariableInfo, List<MethodInfo>>>();
            {
                foreach (var item in methodAttr)
                {
                    var attr = generator.DefineVariable(item.GetType());
                    var args = generator.DefineVariable(typeof(ExecutionArgs));

                    var attrMethod = new List<MethodInfo>();
                    attrMethod.Add(item.GetType().GetMethod("OnEntryMethod"));
                    attrMethod.Add(item.GetType().GetMethod("OnErrorMethod"));
                    attrMethod.Add(item.GetType().GetMethod("OnExitsMethod"));

                    variables.Add(new Tuple<CompilerVariableInfo, CompilerVariableInfo, List<MethodInfo>>(attr, args, attrMethod));
                }

                // var attrs = MethodInfo.GetCurrentMethod().GetCustomAttributes(false);
                var attrs = generator.DefineVariable(typeof(List<MemberInterceptionAttribute>));
                generator.Emit(OpCodes.Call, typeof(MethodBase).GetMethod("GetCurrentMethod", BindingFlags.Public | BindingFlags.Static));
                generator.Emit(OpCodes.Call, typeof(MethodBaseExtension).GetMethod("GetInterceptionAttributes"));
                generator.Emit(OpCodes.Stloc, attrs);

                foreach (var item in variables)
                {
                    // var attr = attrs[index] as MemberInterceptionAttribute;
                    generator.Emit(OpCodes.Ldloc, attrs);
                    generator.Emit(OpCodes.Ldc_I4, variables.IndexOf(item));
                    generator.Emit(OpCodes.Callvirt, typeof(List<MemberInterceptionAttribute>).GetMethod("get_Item"));
                    generator.Emit(OpCodes.Stloc, item.Item1);

                    // var args = new ExecutionArgs();
                    generator.Emit(OpCodes.Newobj, item.Item2.VariabType.GetConstructor(Type.EmptyTypes));
                    generator.Emit(OpCodes.Stloc, item.Item2);
                }

                var leaves = generator.BeginExceptionBlock();
                {
                    // Try
                    generator.BeginTryBlock();
                    {
                        foreach (var item in variables)
                        {
                            var local0 = item.Item1;
                            var local1 = item.Item2;
                            var local2 = generator.DefineVariable(typeof(ExecutionStatus));

                            // args.This = this;
                            if (thisArg != null)
                            {
                                generator.Emit(OpCodes.Ldloc, local1);
                                generator.Emit(OpCodes.Ldarg, thisArg);

                                if (thisArg.ArgumentType.IsValueType)
                                {
                                    generator.Emit(OpCodes.Ldobj, thisArg.ArgumentType);
                                    generator.Emit(OpCodes.Box, thisArg.ArgumentType);
                                }

                                generator.Emit(OpCodes.Callvirt, typeof(ExecutionArgs).GetProperty("This").SetMethod);
                            }

                            // args.Method = MethodBase.GetCurrentMethod();
                            generator.Emit(OpCodes.Ldloc, local1);
                            generator.Emit(OpCodes.Call, typeof(MethodBase).GetMethod("GetCurrentMethod", BindingFlags.Public | BindingFlags.Static));
                            generator.Emit(OpCodes.Callvirt, local1.VariabType.GetProperty("Method").SetMethod);

                            // args.Arguments = new List<Object>();
                            this.Template1(generator, local0, local1);

                            // args.Arguments.Add(param);
                            this.Template2(generator, local0, local1, thisArg);

                            // attr.OnEntryMethod(args);
                            this.Template3(generator, local0, local1, item.Item3[0]);

                            // var status = args.Status;
                            this.Template4(generator, local1, local2);

                            //if (status.HasFlag(ExecutionStatus.ExecutionFailed))
                            //    return;
                            this.Template5(generator, local2, ExecutionStatus.ExecutionFailed, leaves, true);

                            //if (!status.HasFlag(ExecutionStatus.EntryMethodSuccess))
                            //    return;
                            this.Template5(generator, local2, ExecutionStatus.EntryMethodSuccess, leaves, false);
                        }

                        generator.AppendCodes(methodInfo);

                        if (returnVar != null)
                            generator.Emit(OpCodes.Stloc, returnVar);

                        generator.EndTryBlock();
                    }
                    // Catch
                    generator.BeginCatchBlock(exceptionVar);
                    {
                        generator.Emit(OpCodes.Stloc, exceptionVar);

                        foreach (var item in variables)
                        {
                            var local0 = item.Item1;
                            var local1 = item.Item2;
                            var local3 = exceptionVar;
                            var local4 = generator.DefineVariable(typeof(ExecutionStatus));

                            //  args.Exception = exception;
                            generator.Emit(OpCodes.Ldloc, local1);
                            generator.Emit(OpCodes.Ldloc, local3);
                            generator.Emit(OpCodes.Callvirt, typeof(ExecutionArgs).GetProperty("Exception").SetMethod);

                            // args.Arguments = new List<Object>();
                            this.Template1(generator, local0, local1);

                            // args.Arguments.Add(param);
                            this.Template2(generator, local0, local1, thisArg);

                            // attr.OnErrorMethod(args);
                            this.Template3(generator, local0, local1, item.Item3[1]);

                            // var status = args.Status;
                            this.Template4(generator, local1, local4);

                            //if (status.HasFlag(ExecutionStatus.ExecutionFailed))
                            //    return;
                            this.Template5(generator, local4, ExecutionStatus.ExecutionFailed, leaves, true);

                            //if (!status.HasFlag(ExecutionStatus.ErrorMethodSuccess))
                            //    return;
                            this.Template5(generator, local4, ExecutionStatus.ErrorMethodSuccess, leaves, false);
                        }

                        generator.EndCatchBlock();
                    }
                    // Finally
                    generator.BeginFinallyBlock();
                    {
                        foreach (var item in variables)
                        {
                            var local0 = item.Item1;
                            var local1 = item.Item2;

                            // args.Return = return;
                            if (returnVar != null)
                            {
                                generator.Emit(OpCodes.Ldloc, local1);
                                generator.Emit(OpCodes.Ldloc, returnVar);

                                if (returnVar.VariabType.IsValueType)
                                    generator.Emit(OpCodes.Box, returnVar.VariabType);

                                generator.Emit(OpCodes.Callvirt, typeof(ExecutionArgs).GetProperty("Return").SetMethod);
                            }

                            // args.Arguments = new List<Object>();
                            this.Template1(generator, local0, local1);

                            // args.Arguments.Add(param);
                            this.Template2(generator, local0, local1, thisArg);

                            // attr.OnExitsMethod(args);
                            this.Template3(generator, local0, local1, item.Item3[2]);
                        }

                        generator.EndFinallyBlock();
                    }

                    generator.EndExceptionBlock();
                }

                if (returnVar != null)
                    generator.Emit(OpCodes.Ldloc, returnVar);

                generator.Emit(OpCodes.Ret);
            }

            // 重置方法信息
            compilerMethodInfo.SetCodeData(generator.GetCodeData());
            compilerMethodInfo.SetStackSize(generator.GetStackSize());
            compilerMethodInfo.SetClauseInfo(generator.GetClauseInfo());
            compilerMethodInfo.SetSignatureData(generator.GetSignatureData());

            return true;
        }
        #endregion
    }
}
