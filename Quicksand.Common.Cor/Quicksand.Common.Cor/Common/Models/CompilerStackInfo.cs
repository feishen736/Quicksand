
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
    /// 堆栈信息
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public class CompilerStackInfo
    {
        #region 属性
        /// <summary>
        /// 堆栈大小
        /// </summary>
        public Int32 MinStackSize { get; internal set; }
        /// <summary>
        /// 堆栈大小
        /// </summary>
        public Int32 MaxStackSize { get; internal set; }
        /// <summary>
        /// 堆栈大小
        /// </summary>
        public Int32 CurStackSize { get; internal set; }
        #endregion

        #region 构造
        internal CompilerStackInfo() { }
        #endregion

        #region 方法
        /// <summary>
        /// 更新堆栈
        /// </summary>
        /// <param name="stackSize">堆栈大小</param>
        public void UpdateStackSize(OpCode opCode, Int32 stackChanage)
        {
            this.CurStackSize += stackChanage;

            if (this.CurStackSize > this.MinStackSize)
                this.MinStackSize = this.CurStackSize;
            else if (CurStackSize < 0)
                this.CurStackSize = 0;

            if (opCode.GetEndsUncondJmpBlk())
            {
                this.MaxStackSize += this.MinStackSize;
                MinStackSize = 0;
                CurStackSize = 0;
            }
        }
        #endregion
    }
}
