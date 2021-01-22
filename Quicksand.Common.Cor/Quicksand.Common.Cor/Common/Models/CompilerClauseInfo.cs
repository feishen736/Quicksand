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
    /// 子句信息
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public class CompilerClauseInfo
    {
        #region 属性
        /// <summary>
        /// 异常类型
        /// </summary>
        public ExceptionHandlingClauseOptions Flags { get; internal set; }
        /// <summary>
        /// 尝试偏移
        /// </summary>
        public Int32 TryOffset { get; internal set; }
        /// <summary>
        /// 尝试长度
        /// </summary>
        public Int32 TryLength { get; internal set; }
        /// <summary>
        /// 捕获偏移
        /// </summary>
        public Int32 HandlerOffset { get; internal set; }
        /// <summary>
        /// 捕获长度
        /// </summary>
        public Int32 HandlerLength { get; internal set; }
        /// <summary>
        /// 令牌偏移
        /// </summary>
        public Int32 TokenOrOffset { get; internal set; }

        /// <summary>
        /// 标签信息
        /// </summary>
        internal List<CompilerLabelInfo> Labels { get; set; }
        /// <summary>
        /// 子句信息
        /// </summary>
        internal List<CompilerClauseInfo> Clauses { get; set; }
        /// <summary>
        /// 捕获子句
        /// </summary>
        internal Stack<CompilerClauseInfo> CatchClauses { get; set; }
        /// <summary>
        /// 最后子句
        /// </summary>
        internal Stack<CompilerClauseInfo> FinallyClauses { get; set; }
        #endregion

        #region 构造
        internal CompilerClauseInfo()
        {
            this.Clauses = new List<CompilerClauseInfo>();

            this.CatchClauses = new Stack<CompilerClauseInfo>();
            this.FinallyClauses = new Stack<CompilerClauseInfo>();
        }
        #endregion
    }
}
