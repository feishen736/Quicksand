using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    /// <summary>
    /// 参数信息
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public sealed class CompilerArgumentInfo
    {
        #region 属性
        /// <summary>
        /// 参数类型
        /// </summary>
        public Type ArgumentType { get; internal set; }
        /// <summary>
        /// 参数索引
        /// </summary>
        public Int32 ArgumentIndex { get; internal set; }
        /// <summary>
        /// 参数特性
        /// </summary>
        public ParameterAttributes ArgumentAttributes { get; internal set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        internal List<CompilerArgumentInfo> Arguments { get; set; }
        #endregion

        #region 构造
        internal CompilerArgumentInfo()
        {
            this.Arguments = new List<CompilerArgumentInfo>();
        }
        #endregion
    }
}
