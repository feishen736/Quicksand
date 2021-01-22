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
    /// 变量信息
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public sealed class CompilerVariableInfo
    {
        #region 属性
        /// <summary>
        /// 变量类型
        /// </summary>
        public Type VariabType { get; internal set; }
        /// <summary>
        /// 变量索引
        /// </summary>
        public Int32 VariabIndex { get; internal set; }

        /// <summary>
        /// 变量列表
        /// </summary>
        internal List<CompilerVariableInfo> Variables { get; set; }
        #endregion

        #region 构造
        internal CompilerVariableInfo()
        {
            this.Variables = new List<CompilerVariableInfo>();
        }
        #endregion
    }
}
