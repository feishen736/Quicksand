using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    /// <summary>
    /// 编码信息
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public class CompilerCodeInfo
    {
        #region 属性
        /// <summary>
        /// 编码信息
        /// </summary>
        public List<Byte> Codes { get; internal set; }
        #endregion

        #region 构造
        internal CompilerCodeInfo()
        {
            this.Codes = new List<Byte>();
        }
        #endregion
    }
}
