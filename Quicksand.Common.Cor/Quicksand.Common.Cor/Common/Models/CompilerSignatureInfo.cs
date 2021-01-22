using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    /// <summary>
    /// 签名信息
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public sealed class CompilerSignatureInfo
    {
        #region 构造
        private CompilerSignatureInfo(Module ownerModule)
        {
            this.moduleInfo = ownerModule;
            this.sigData = new List<Byte>();
        }
        #endregion

        #region 字段
        // 模块信息
        private Module moduleInfo;

        // 签名类型
        private Byte sigCall;
        // 签名统计
        private Byte sigCount;
        // 签名数据
        private List<Byte> sigData;
        #endregion

        #region 方法
        /// <summary>
        /// 获取变量签名
        /// </summary>
        /// <param name="ownerModule">模块信息</param>
        public static CompilerSignatureInfo GetVariableSigInfo(Module ownerModule)
        {
            var sigInfo = new CompilerSignatureInfo(ownerModule);
            sigInfo.sigCall = (Byte)SigCallingType.LocalSig;
            sigInfo.sigCount = 0;

            return sigInfo;
        }
        /// <summary>
        /// 获取类型签名
        /// </summary>
        /// <param name="ownerModule">模块信息</param>
        /// <param name="typeInfo">类型信息</param>
        /// <returns>类型签名</returns>
        public static CompilerSignatureInfo GetTypeSigInfo(Module ownerModule, Type typeInfo)
        {
            var sigInfo = new CompilerSignatureInfo(ownerModule);
            sigInfo.sigCall = (Byte)SigCallingType.Default;
            sigInfo.sigCount = 0;

            return sigInfo;
        }
        /// <summary>
        /// 获取方法签名
        /// </summary>
        /// <param name="ownerModule">模块信息</param>
        /// <param name="methodInfo">方法信息</param>
        /// <returns></returns>
        public static CompilerSignatureInfo GetMethodSigInfo(Module ownerModule, MethodInfo methodInfo)
        {
            var sigInfo = new CompilerSignatureInfo(ownerModule);

            if (methodInfo.CallingConvention.HasFlag(CallingConventions.VarArgs))
                sigInfo.sigCall |= (Byte)SigCallingType.Vararg;
            if (methodInfo.CallingConvention.HasFlag(CallingConventions.Standard))
                sigInfo.sigCall |= (Byte)SigCallingType.StdCall;
            if (methodInfo.CallingConvention.HasFlag(CallingConventions.HasThis))
                sigInfo.sigCall |= (Byte)SigCallingType.HasThis;

            return sigInfo;
        }
        /// <summary>
        /// 获取字段签名
        /// </summary>
        /// <param name="ownerModule">模块信息</param>
        /// <param name="fieldInfo">字段信息</param>
        /// <returns></returns>
        public static CompilerSignatureInfo GetFieldSignature(Module ownerModule, PropertyInfo fieldInfo)
        {
            var sigInfo = new CompilerSignatureInfo(ownerModule);
            sigInfo.sigCall = (Byte)SigCallingType.Field;

            return sigInfo;
        }
        /// <summary>
        /// 获取属性签名
        /// </summary>
        /// <param name="ownerModule">模块信息</param>
        /// <param name="propertyInfo">属性信息</param>
        /// <returns></returns>
        public static CompilerSignatureInfo GetPropertySignature(Module ownerModule, PropertyInfo propertyInfo)
        {
            var sigInfo = new CompilerSignatureInfo(ownerModule);
            sigInfo.sigCall = (Byte)SigCallingType.Property;

            return sigInfo;
        }

        /// <summary>
        /// 添加签名数据
        /// </summary>
        /// <param name="typeInfo">类型信息</param>
        /// <param name="typeToken">类型令牌</param>
        public void AddSigData(Type typeInfo)
        {
            this.sigCount++;

            this.sigData.AddRange(moduleInfo.GetTypeMetadataSignature(typeInfo));
        }
        /// <summary>
        /// 获取签名数据
        /// </summary>
        /// <returns>签名数据</returns>
        public List<Byte> GetSigData()
        {
            if ((SigCallingType)this.sigCall == SigCallingType.Default)
                return this.sigData;

            this.sigData.Insert(0, this.sigCall);
            this.sigData.Insert(1, this.sigCount);

            return this.sigData;
        }
        #endregion
    }
}
