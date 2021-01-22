using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Aop.Common
{
    /// <summary>
    /// 执行参数
    /// </summary>
    public sealed class ExecutionArgs
    {
        #region 构造
        public ExecutionArgs()
        {
            this.Arguments = new List<Object>();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 执行参数
        /// </summary>
        public Object Parameter { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public ExecutionStatus Status { get; set; }

        /// <summary>
        /// 拦截实例
        /// </summary>
        public Object This { get; set; }
        /// <summary>
        /// 拦截方法
        /// </summary>
        public MethodBase Method { get; set; }
        /// <summary>
        /// 拦截结果
        /// </summary>
        public Object Return { get; set; }
        /// <summary>
        /// 拦截参数
        /// </summary>
        public List<Object> Arguments { get; set; }
        /// <summary>
        /// 拦截异常
        /// </summary>
        public Exception Exception { get; set; }
        #endregion
    }
}
