using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    /// <summary>
    /// 标签信息
    /// </summary>
    public sealed class CompilerLabelInfo
    {
        #region 属性
        /// <summary>
        /// 标签大小
        /// </summary>
        public const Int32 Size = 5;
        /// <summary>
        /// 标签偏移
        /// </summary>
        public Int32 Offset { get; internal set; }
        #endregion

        #region 构造
        internal CompilerLabelInfo()
        {

        }
        #endregion
    }
}
