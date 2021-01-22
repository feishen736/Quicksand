



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

namespace Quicksand.Common.Cor.Common
{
    /// <summary>
    /// 编译方法信息
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public sealed class CompilerMethodInfo
    {
        #region 构造
        static CompilerMethodInfo()
        {
            RuntimeHelpers.PrepareMethod(typeof(List<CorInfoClauseInfo>).GetConstructor(Type.EmptyTypes).MethodHandle);
        }
        internal CompilerMethodInfo()
        {
            this.MethodInfo = null;
            this.MethodDesc = new CorInfoMethodInfo();
            this.ClauseInfo = new List<CorInfoClauseInfo>();
        }
        #endregion

        #region 字段
        // 方法描述
        internal MethodBase MethodInfo;
        // 方法信息
        internal CorInfoMethodInfo MethodDesc;
        // 子句信息
        internal List<CorInfoClauseInfo> ClauseInfo;
        #endregion

        #region 方法
        /// <summary>
        /// 获取方法信息
        /// </summary>
        /// <returns></returns>
        public MethodBase GetMethodInfo()
        {
            return this.MethodInfo;
        }
        /// <summary>
        /// 获取方法编辑
        /// </summary>
        /// <returns>方法</returns>
        public CompilerILGenerator GetILGenerator()
        {
            return new CompilerILGenerator(this.MethodInfo.Module);
        }

        /// <summary>
        /// 获取编码数据
        /// </summary>
        /// <returns>编码数组</returns>
        public Byte[] GetCodeData()
        {
            var codes = new Byte[this.MethodDesc.ILSize];

            Marshal.Copy(this.MethodDesc.ILCode, codes, 0, codes.Length);

            return codes;
        }
        /// <summary>
        /// 设置编码数据
        /// </summary>
        /// <param name="codes">编码数组</param>
        public void SetCodeData(Byte[] codes, UInt32 protectValue = 0)
        {
            this.MethodDesc.ILSize = codes.Length;
            this.MethodDesc.ILCode = Marshal.AllocHGlobal(codes.Length);

            Kernel32.VirtualProtect(this.MethodDesc.ILCode, (UInt32)IntPtr.Size, Protection.PageExecuteReadWrite, out protectValue);
            Marshal.Copy(codes, 0, this.MethodDesc.ILCode, this.MethodDesc.ILSize);
            Kernel32.VirtualProtect(this.MethodDesc.ILCode, (UInt32)IntPtr.Size, (Protection)protectValue, out protectValue);
        }

        /// <summary>
        /// 获取堆栈大小
        /// </summary>
        /// <returns>堆栈大小</returns>
        public Int32 GetStackSize()
        {
            return this.MethodDesc.MaxStack;
        }
        /// <summary>
        /// 设置堆栈大小
        /// </summary>
        /// <param name="maxStackSize">堆栈大小</param>
        public void SetStackSize(Int32 maxStackSize)
        {
            this.MethodDesc.MaxStack = maxStackSize;
        }

        /// <summary>
        /// 获取签名数据
        /// </summary>
        /// <returns>签名标识</returns>
        public Byte[] GetSignatureData()
        {
            var sigData = new Byte[this.MethodDesc.Locals.CbSig];

            if (sigData.Length > 0)
                Marshal.Copy(this.MethodDesc.Locals.PSig, sigData, 0, this.MethodDesc.Locals.CbSig);

            return sigData;
        }
        /// <summary>
        /// 设置签名数据
        /// </summary>
        /// <param name="signatureData">签名数据</param>
        public void SetSignatureData(Byte[] signatureData)
        {
            this.MethodDesc.Locals.CbSig = signatureData.Length;
            this.MethodDesc.Locals.NumArgs = signatureData[1];
            this.MethodDesc.Locals.PSig = this.MethodInfo.Module.GetSignatureMetadataPointer(signatureData);
            this.MethodDesc.Locals.Args = this.MethodDesc.Locals.PSig + 2;
        }

        /// <summary>
        /// 获取子句信息
        /// </summary>
        /// <returns>子句信息</returns>
        public List<ExceptionHandler> GetClauseInfo()
        {
            var clauses = new List<ExceptionHandler>();

            if (this.ClauseInfo.Count == 0)
                foreach (var item in this.MethodInfo.GetMethodBody().ExceptionHandlingClauses)
                    clauses.Add(new ExceptionHandler(item.TryOffset, item.TryLength, item.FilterOffset, item.HandlerOffset, item.HandlerLength, item.Flags, item.FilterOffset));
            else
                foreach (var item in this.ClauseInfo)
                    clauses.Add(new ExceptionHandler(item.TryOffset, item.TryLength, item.TokenOrOffset, item.HandlerOffset, item.HandlerLength, (ExceptionHandlingClauseOptions)item.Flags, item.TokenOrOffset));

            return clauses;
        }
        /// <summary>
        /// 设置子句信息
        /// </summary>
        /// <param name="clauseInfo">子句信息</param>
        public void SetClauseInfo(List<CompilerClauseInfo> clauses)
        {
            this.ClauseInfo.Clear();

            foreach (var item in clauses)
            {
                var clause = new CorInfoClauseInfo();
                clause.Flags = (CorInfoClauseFlags)item.Flags;
                clause.TryOffset = item.TryOffset;
                clause.TryLength = item.TryLength;
                clause.HandlerOffset = item.HandlerOffset;
                clause.HandlerLength = item.HandlerLength;
                clause.TokenOrOffset = item.TokenOrOffset;

                this.ClauseInfo.Add(clause);
            }

            this.MethodDesc.EHcount = this.ClauseInfo.Count;
        }
        #endregion
    }
}
