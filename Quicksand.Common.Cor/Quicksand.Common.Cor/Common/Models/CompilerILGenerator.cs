
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    /// <summary>
    /// 编译方法生成
    /// </summary>
    public sealed class CompilerILGenerator
    {
        #region 字段
        // 模块信息
        private Module moduleInfo;

        // 编码信息
        private CompilerCodeInfo codeInfo;
        // 堆栈信息
        private CompilerStackInfo stackInfo;
        // 子句信息
        private CompilerClauseInfo clauseInfo;
        // 变量信息
        private CompilerVariableInfo variableInfo;
        // 参数信息
        private CompilerArgumentInfo argumentInfo;
        #endregion

        #region 构造
        public CompilerILGenerator(Module ownerModule)
        {
            this.moduleInfo = ownerModule;

            this.codeInfo = new CompilerCodeInfo();
            this.stackInfo = new CompilerStackInfo();
            this.clauseInfo = new CompilerClauseInfo();
            this.variableInfo = new CompilerVariableInfo();
            this.argumentInfo = new CompilerArgumentInfo();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取类型令牌
        /// </summary>
        /// <param name="typeInfo">类型信息</param>
        /// <returns>类型令牌</returns>
        private Int32 GetMetadataToken(Type typeInfo, Boolean genericInst = false)
        {
            var metadataToken = 0;

            // 泛型类型
            if (typeInfo.IsGenericType && genericInst == false)
            {
                foreach (var item in typeInfo.GenericTypeArguments)
                    this.GetMetadataToken(item);

                if (metadataToken == 0)
                    this.moduleInfo.GetTypeSpecMetadataToken(typeInfo);
                if (metadataToken == 0)
                    this.moduleInfo.DefineTypeSpecMetadataToken(typeInfo);

                metadataToken = this.GetMetadataToken(typeInfo.GetGenericTypeDefinition(), true);
            }
            // 常规类型
            else
            {
                if (metadataToken == 0)
                    metadataToken = this.moduleInfo.GetTypeMetadataToken(typeInfo);
                if (metadataToken == 0)
                    metadataToken = this.moduleInfo.DefineTypeRefMetadataToken(typeInfo);
            }

            return metadataToken;
        }
        /// <summary>
        /// 获取成员令牌
        /// </summary>
        /// <param name="memberInfo">成员信息</param>
        /// <returns>成员令牌</returns>
        private Int32 GetMetadataToken(MethodBase methodInfo)
        {
            if (methodInfo.DeclaringType != null)
                this.GetMetadataToken(methodInfo.ReflectedType);

            foreach (var item in methodInfo.GetParameters())
                this.GetMetadataToken(item.ParameterType);

            var metadataToken = 0;

            if (methodInfo.IsGenericMethod)
            {
                // 未实现
            }
            else
            {
                if (metadataToken == 0)
                    metadataToken = this.moduleInfo.GetMemberMetadataToken(methodInfo);
                if (metadataToken == 0)
                    metadataToken = this.moduleInfo.DefineMemberRefMetadataToken(methodInfo);
            }

            return metadataToken;
        }
        /// <summary>
        /// 获取字符令牌
        /// </summary>
        /// <param name="memberInfo">字符信息</param>
        /// <returns>字符令牌</returns>
        private Int32 GetMetadataToken(String value)
        {
            var metadataToken = 0;

            if (metadataToken == 0)
                this.moduleInfo.GetStringMetadataToken(value);
            if (metadataToken == 0)
                this.moduleInfo.DefineStringMetadataToken(value);

            return metadataToken;
        }

        /// <summary>
        /// 开始异常区域
        /// </summary>
        public List<CompilerLabelInfo> BeginExceptionBlock()
        {
            this.clauseInfo.Labels = new List<CompilerLabelInfo>();

            return this.clauseInfo.Labels;
        }
        /// <summary>
        /// 结束异常区域
        /// </summary>
        public void EndExceptionBlock()
        {
            foreach (var item in this.clauseInfo.Labels)
                this.Emit(OpCodes.Leave, item);

        }
        /// <summary>
        /// 开始尝试区域
        /// </summary>
        public void BeginTryBlock()
        {
            var catchClause = new CompilerClauseInfo() { TryOffset = this.codeInfo.Codes.Count };
            var finallyClause = new CompilerClauseInfo() { TryOffset = this.codeInfo.Codes.Count };

            this.clauseInfo.CatchClauses.Push(catchClause);
            this.clauseInfo.FinallyClauses.Push(finallyClause);
        }
        /// <summary>
        /// 结束尝试区域
        /// </summary>
        public void EndTryBlock()
        {
            this.clauseInfo.Labels.Add(this.DefineLabel());

            var catchClause = this.clauseInfo.CatchClauses.Peek();
            var finallyClause = this.clauseInfo.FinallyClauses.Peek();

            catchClause.TryLength = this.codeInfo.Codes.Count - catchClause.TryOffset;
            finallyClause.TryLength = this.codeInfo.Codes.Count - finallyClause.TryOffset;
        }
        /// <summary>
        /// 开始捕获区域
        /// </summary>
        /// <param name="exceptionVariable">异常变量</param>
        public void BeginCatchBlock(CompilerVariableInfo exceptionVariable)
        {
            var catchClause = this.clauseInfo.CatchClauses.Pop() ?? this.clauseInfo.Clauses.Last();
            catchClause.HandlerOffset = this.codeInfo.Codes.Count;
            catchClause.TokenOrOffset = this.GetMetadataToken(exceptionVariable.VariabType);

            this.clauseInfo.CatchClauses.Push(catchClause);
        }
        /// <summary>
        /// 结束捕获区域
        /// </summary>
        public void EndCatchBlock()
        {
            this.clauseInfo.Labels.Add(this.DefineLabel());

            var catchClause = this.clauseInfo.CatchClauses.Pop();
            var finallyClause = this.clauseInfo.FinallyClauses.Pop();

            catchClause.Flags = ExceptionHandlingClauseOptions.Clause;
            catchClause.HandlerLength = this.codeInfo.Codes.Count - catchClause.HandlerOffset;

            finallyClause.HandlerOffset = catchClause.HandlerOffset;
            finallyClause.HandlerLength = catchClause.HandlerLength;

            this.clauseInfo.Clauses.Add(catchClause);
            this.clauseInfo.FinallyClauses.Push(finallyClause);
        }
        /// <summary>
        /// 开始最后区域
        /// </summary>
        public void BeginFinallyBlock()
        {
            var finallyClause = this.clauseInfo.FinallyClauses.Pop();
            finallyClause.TokenOrOffset = 0;
            finallyClause.HandlerOffset = this.codeInfo.Codes.Count;
            finallyClause.TryLength = this.codeInfo.Codes.Count - finallyClause.TryOffset;

            this.clauseInfo.FinallyClauses.Push(finallyClause);
        }
        /// <summary>
        /// 结束最后区域
        /// </summary>
        public void EndFinallyBlock()
        {
            this.Emit(OpCodes.Endfinally);

            var finallyClause = this.clauseInfo.FinallyClauses.Pop();
            finallyClause.Flags = ExceptionHandlingClauseOptions.Finally;
            finallyClause.HandlerLength = this.codeInfo.Codes.Count - finallyClause.HandlerOffset;

            this.clauseInfo.Clauses.Add(finallyClause);
        }

        /// <summary>
        /// 发出操作编码
        /// </summary>
        /// <param name="opCode">操作编码</param>
        public void Emit(OpCode opCode)
        {
            this.codeInfo.Codes.Add(opCode.GetValue());

            this.stackInfo.UpdateStackSize(opCode, opCode.GetStackChanage());
        }
        /// <summary>
        /// 发出操作编码
        /// </summary>
        /// <param name="opCode">操作编码</param>
        /// <param name="value">枚举数值</param>
        public void Emit(OpCode opCode, Enum value)
        {
            if (opCode.Equals(OpCodes.Ldc_I4))
                opCode = this.GetLdc_I4(Convert.ToInt32(value));

            this.Emit(opCode, Convert.ToInt32(value));
        }
        /// <summary>
        /// 发出指令操作
        /// </summary>
        /// <param name="opCode">操作编码</param>
        /// <param name="value">字节数值</param>
        public void Emit(OpCode opCode, Byte value)
        {
            this.Emit(opCode);

            this.codeInfo.Codes.Add(value);
        }
        /// <summary>
        /// 发出操作编码
        /// </summary>
        /// <param name="opCode">操作编码</param>
        /// <param name="value">整型数值</param>
        public void Emit(OpCode opCode, Int32 value)
        {
            this.Emit(opCode);

            switch (opCode.OperandType)
            {
                case OperandType.InlineBrTarget:
                case OperandType.InlineField:
                case OperandType.InlineI:
                case OperandType.InlineMethod:
                case OperandType.InlineSig:
                case OperandType.InlineString:
                case OperandType.InlineSwitch:
                case OperandType.InlineTok:
                case OperandType.InlineType:
                    {
                        this.codeInfo.Codes.Add((Byte)(value >> 0));
                        this.codeInfo.Codes.Add((Byte)(value >> 8));
                        this.codeInfo.Codes.Add((Byte)(value >> 16));
                        this.codeInfo.Codes.Add((Byte)(value >> 24));
                        break;
                    }
                case OperandType.InlineVar:
                    {
                        this.codeInfo.Codes.Add((Byte)(value >> 0));
                        this.codeInfo.Codes.Add((Byte)(value >> 8));
                        break;
                    }
                case OperandType.ShortInlineBrTarget:
                case OperandType.ShortInlineVar:
                case OperandType.ShortInlineI:
                    {
                        this.codeInfo.Codes.Add((Byte)(value >> 0));
                        break;
                    }
                default:
                    break;
            }
        }
        /// <summary>
        /// 发出操作编码
        /// </summary>
        /// <param name="opCode">操作编码</param>
        /// <param name="value">字符数值</param>
        public void Emit(OpCode opCode, String value)
        {
            this.Emit(opCode, this.GetMetadataToken(value)); 
        }
        /// <summary>
        /// 发出操作编码
        /// </summary>
        /// <param name="opCode">操作编码</param>
        /// <param name="value">布尔数值</param>
        public void Emit(OpCode opCode, Boolean value)
        {
            if (value)
                this.Emit(opCode, 1);
            else
                this.Emit(opCode, 0);
        }
        /// <summary>
        /// 发出操作编码
        /// </summary>
        /// <param name="opCode">操作编码</param>
        /// <param name="typeInfo">类型信息</param>
        public void Emit(OpCode opCode, Type typeInfo)
        {
            var metadataToken = 0;

            if (metadataToken == 0)
                metadataToken = this.moduleInfo.GetTypeMetadataToken(typeInfo);
            if (metadataToken == 0)
                metadataToken = this.moduleInfo.DefineTypeRefMetadataToken(typeInfo);

            this.Emit(opCode, metadataToken);
        }
        /// <summary>
        /// 发出操作编码
        /// </summary>
        /// <param name="opCode">操作编码</param>
        /// <param name="memberInfo">成员信息</param>
        public void Emit(OpCode opCode, MethodBase methodInfo)
        {
            var stackCahnage = 0;
            var metadataToken = this.GetMetadataToken(methodInfo);

            var method = methodInfo as MethodInfo;
            var constructor = methodInfo as ConstructorInfo;

            if (method != null)
            {
                var parameterInfo = method.GetParameters().ToList();

                if (method.ReturnType != typeof(void))
                    stackCahnage++;
                if ((method.CallingConvention & CallingConventions.HasThis) == CallingConventions.HasThis)
                    stackCahnage--;
                if (!(method is ISymbolMethod) && !method.IsStatic && !(opCode.Equals(OpCodes.Newobj)))
                    stackCahnage--;

                stackCahnage -= parameterInfo.Count;
            }
            else if (constructor != null)
            {
                var parameterInfo = constructor.GetParameters().ToList();

                if (opCode.StackBehaviourPush == StackBehaviour.Varpush)
                    stackCahnage++;
                if (opCode.StackBehaviourPop == StackBehaviour.Varpop)
                    stackCahnage -= parameterInfo.Count;
            }

            this.Emit(opCode, metadataToken);
            this.stackInfo.UpdateStackSize(opCode, stackCahnage);
        }
        /// <summary>
        /// 发出操作编码
        /// </summary>
        /// <param name="opCode">操作编码</param>
        /// <param name="labelInfo">标签信息</param>
        public void Emit(OpCode opCode, CompilerLabelInfo labelInfo)
        {
            var offset = labelInfo.Offset;
            var length = this.codeInfo.Codes.Count - offset - CompilerLabelInfo.Size;

            this.codeInfo.Codes[offset] = opCode.GetValue();

            this.codeInfo.Codes[offset + 1] = (Byte)(length >> 0);
            this.codeInfo.Codes[offset + 2] = (Byte)(length >> 8);
            this.codeInfo.Codes[offset + 3] = (Byte)(length >> 16);
            this.codeInfo.Codes[offset + 4] = (Byte)(length >> 24);
        }
        /// <summary>
        /// 发出操作编码
        /// </summary>
        /// <param name="opCode">操作编码</param>
        /// <param name="variableInfo">变量信息</param>
        public void Emit(OpCode opCode, CompilerVariableInfo variableInfo)
        {
            if (opCode.Equals(OpCodes.Ldloc))
                opCode = this.GetLdloc(variableInfo.VariabIndex);
            if (opCode.Equals(OpCodes.Stloc))
                opCode = this.GetStloc(variableInfo.VariabIndex);
            if (opCode.Equals(OpCodes.Ldloca))
                opCode = OpCodes.Ldloca_S;

            this.Emit(opCode, variableInfo.VariabIndex);
        }
        /// <summary>
        /// 发出操作编码
        /// </summary>
        /// <param name="opCode">操作编码</param>
        /// <param name="variableInfo">参数信息</param>
        public void Emit(OpCode opCode, CompilerArgumentInfo argumentInfo)
        {
            if (opCode.Equals(OpCodes.Ldarg))
                opCode = this.GetLdarg(argumentInfo.ArgumentIndex);
            if (opCode.Equals(OpCodes.Ldarga))
                opCode = OpCodes.Ldloc_S;

            this.Emit(opCode, argumentInfo.ArgumentIndex);
        }

        /// <summary>
        /// 获取操作编码
        /// </summary>
        /// <param name="typeCode">类型</param>
        /// <returns>操作编码</returns>
        public OpCode GetLdind(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Object:
                case TypeCode.String:
                case TypeCode.DBNull:
                    return OpCodes.Ldind_Ref;
                case TypeCode.Char:
                    return OpCodes.Ldind_U2;
                case TypeCode.SByte:
                    return OpCodes.Ldind_I1;
                case TypeCode.Int16:
                    return OpCodes.Ldind_I2;
                case TypeCode.Int32:
                    return OpCodes.Ldind_I4;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return OpCodes.Ldind_I8;
                case TypeCode.Byte:
                case TypeCode.Boolean:
                    return OpCodes.Ldind_U1;
                case TypeCode.UInt16:
                    return OpCodes.Ldind_U2;
                case TypeCode.UInt32:
                    return OpCodes.Ldind_U4;
                case TypeCode.Single:
                    return OpCodes.Ldind_R4;
                case TypeCode.Double:
                    return OpCodes.Ldind_R8;
                default:
                    return OpCodes.Ldobj;
            }
        }
        /// <summary>
        /// 获取操作编码
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>操作编码</returns>
        public OpCode GetLdc_I4(Int32 value)
        {
            switch (value)
            {
                case -1:
                    return OpCodes.Ldc_I4_M1;
                case 0:
                    return OpCodes.Ldc_I4_0;
                case 1:
                    return OpCodes.Ldc_I4_1;
                case 2:
                    return OpCodes.Ldc_I4_2;
                case 3:
                    return OpCodes.Ldc_I4_3;
                case 4:
                    return OpCodes.Ldc_I4_4;
                case 5:
                    return OpCodes.Ldc_I4_5;
                case 6:
                    return OpCodes.Ldc_I4_6;
                case 7:
                    return OpCodes.Ldc_I4_7;
                case 8:
                    return OpCodes.Ldc_I4_8;
                default:
                    return OpCodes.Ldc_I4_S;
            }
        }
        /// <summary>
        /// 获取操作编码
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>操作编码</returns>
        public OpCode GetLdloc(Int32 index)
        {
            switch (index)
            {
                case 0:
                    return OpCodes.Ldloc_0;
                case 1:
                    return OpCodes.Ldloc_1;
                case 2:
                    return OpCodes.Ldloc_2;
                case 3:
                    return OpCodes.Ldloc_3;
                default:
                    return OpCodes.Ldloc_S;
            }
        }
        /// <summary>
        /// 获取操作编码
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>操作编码</returns>
        public OpCode GetStloc(Int32 index)
        {
            switch (index)
            {
                case 0:
                    return OpCodes.Stloc_0;
                case 1:
                    return OpCodes.Stloc_1;
                case 2:
                    return OpCodes.Stloc_2;
                case 3:
                    return OpCodes.Stloc_3;
                default:
                    return OpCodes.Stloc_S;
            }
        }
        /// <summary>
        /// 获取操作编码
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>操作编码</returns>
        public OpCode GetLdarg(Int32 index)
        {
            switch (index)
            {
                case 0:
                    return OpCodes.Ldarg_0;
                case 1:
                    return OpCodes.Ldarg_1;
                case 2:
                    return OpCodes.Ldarg_2;
                case 3:
                    return OpCodes.Ldarg_3;
                default:
                    return OpCodes.Ldloc_S;
            }
        }

        /// <summary>
        /// 定义标签信息
        /// </summary>
        /// <returns>标签信息</returns>
        public CompilerLabelInfo DefineLabel()
        {
            var label = new CompilerLabelInfo() { Offset = this.codeInfo.Codes.Count };

            // 填充编码
            for (int i = 0; i < CompilerLabelInfo.Size; i++)
                this.Emit(OpCodes.Nop);

            return label;
        }
        /// <summary>
        /// 定义变量信息
        /// </summary>
        /// <param name="variableType">变量类型</param>
        /// <returns>变量信息</returns>
        public CompilerVariableInfo DefineVariable(Type variableType)
        {
            var variable = new CompilerVariableInfo();
            variable.VariabType = variableType;
            variable.VariabIndex = this.variableInfo.Variables.Count;

            this.variableInfo.Variables.Add(variable);

            return variable;
        }
        /// <summary>
        /// 定义参数信息
        /// </summary>
        /// <param name="variableType">参数类型</param>
        /// <returns>参数信息</returns>
        public CompilerArgumentInfo DefineArgument(Type argumentType)
        {
            var argument = new CompilerArgumentInfo();
            argument.ArgumentType = argumentType;
            argument.ArgumentIndex = this.argumentInfo.Arguments.Count;

            this.argumentInfo.Arguments.Add(argument);

            return argument;
        }

        /// <summary>
        /// 获取堆栈大小
        /// </summary>
        /// <returns></returns>
        public Int32 GetStackSize()
        {
            return this.stackInfo.MaxStackSize;
        }
        /// <summary>
        /// 获取编码数据
        /// </summary>
        /// <returns>编码数组</returns>
        public Byte[] GetCodeData()
        {
            return codeInfo.Codes.ToArray();
        }
        /// <summary>
        /// 获取变量指针
        /// </summary>
        /// <returns>变量指针</returns>
        public Byte[] GetSignatureData()
        {
            var variableSig = CompilerSignatureInfo.GetVariableSigInfo(moduleInfo);

            foreach (var item in this.variableInfo.Variables)
                variableSig.AddSigData(item.VariabType);

            return variableSig.GetSigData().ToArray();
        }
        /// <summary>
        /// 获取子句信息
        /// </summary>
        /// <returns>子句信息</returns>
        public List<CompilerClauseInfo> GetClauseInfo()
        {
            return this.clauseInfo.Clauses;
        }
        /// <summary>
        /// 获取变量信息
        /// </summary>
        /// <returns>参数信息</returns>
        public List<CompilerVariableInfo> GetVariableInfo()
        {
            return this.variableInfo.Variables;
        }
        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <returns>参数信息</returns>
        public List<CompilerArgumentInfo> GetArgumentInfo()
        {
            return this.argumentInfo.Arguments;
        }

        /// <summary>
        /// 附加编码信息
        /// </summary>
        /// <param name="codes"></param>
        public void AppendCodes(MethodBase methodInfo)
        {
            if (this.moduleInfo != methodInfo.Module)
                throw new Exception("方法信息与当前模块信息作用域不一致!");

            // 方法主体
            var methodBody = methodInfo.GetMethodBody();
            var methodCodes = methodBody.GetILAsByteArray();
            var methodClauses = methodBody.ExceptionHandlingClauses;

            // 附加堆栈
            this.stackInfo.MaxStackSize += methodBody.MaxStackSize;
            // 附加子句
            for (int i = 0; i < methodClauses.Count; i++)
            {
                var ehcType = typeof(ExceptionHandlingClause);
                var flags = (ExceptionHandlingClauseOptions)ehcType.GetField("m_flags", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(methodClauses[i]);
                var tryOffset = (Int32)ehcType.GetField("m_tryOffset", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(methodClauses[i]);
                var tryLength = (Int32)ehcType.GetField("m_tryLength", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(methodClauses[i]);
                var handlerOffset = (Int32)ehcType.GetField("m_handlerOffset", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(methodClauses[i]);
                var handlerLength = (Int32)ehcType.GetField("m_handlerLength", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(methodClauses[i]);
                var tokenOrOffset = (Int32)ehcType.GetField("m_catchMetadataToken", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(methodClauses[i]);

                var clause = new CompilerClauseInfo();
                clause.Flags = flags;
                clause.TryOffset = tryOffset + this.codeInfo.Codes.Count;
                clause.TryLength = tryLength;
                clause.HandlerOffset = handlerOffset + this.codeInfo.Codes.Count;
                clause.HandlerLength = handlerLength;
                clause.TokenOrOffset = tokenOrOffset;

                this.clauseInfo.Clauses.Insert(i, clause);
            }
            // 附加编码
            for (int i = methodCodes.Length - 1; i >= 0; i--)
            {
                if (methodCodes[i] == OpCodes.Nop.GetValue())
                    continue;

                if (methodCodes[i] == OpCodes.Ret.GetValue())
                {
                    this.codeInfo.Codes.AddRange(methodCodes.Take(i));
                    break;
                }
                else
                {
                    this.codeInfo.Codes.AddRange(methodCodes.Take(i + 1));
                    break;
                }
            }
        }
        #endregion
    }
}
