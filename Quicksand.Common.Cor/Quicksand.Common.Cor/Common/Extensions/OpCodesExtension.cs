using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    /// <summary>
    /// OpCodes 扩展方法
    /// </summary>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public static class OpCodesExtension
    {
        /// <summary>
        /// 获取数值
        /// </summary>
        /// <param name="opCode">当前编码</param>
        /// <returns>编码数值</returns>
        public static Byte GetValue(this OpCode opCode)
        {
            if (opCode.Size != 1)
                return (Byte)(opCode.Value >> 8);

            return (Byte)opCode.Value;
        }
        /// <summary>
        /// 获取堆栈
        /// </summary>
        /// <param name="opCode">当前编码</param>
        /// <returns>堆栈大小</returns>
        public static Int32 GetStackChanage(this OpCode opCode)
        {
            var stackChange = opCode.GetType().GetMethod("StackChange", BindingFlags.NonPublic | BindingFlags.Instance);

            return (Int32)stackChange.Invoke(opCode, null);
        }
        /// <summary>
        /// 获取结束
        /// </summary>
        /// <param name="opCode">当前编码</param>
        /// <returns>结束标识</returns>
        public static Boolean GetEndsUncondJmpBlk(this OpCode opCode)
        {
            var stackChange = opCode.GetType().GetMethod("EndsUncondJmpBlk", BindingFlags.NonPublic | BindingFlags.Instance);

            return (Boolean)stackChange.Invoke(opCode, null);
        }
    }
}
