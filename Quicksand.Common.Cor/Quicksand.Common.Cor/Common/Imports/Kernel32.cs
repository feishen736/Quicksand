using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    [Flags]
    internal enum Protection : Int32
    {
        PageNoAccess            = 0x01,
        PageReadWrite           = 0x04,
        PageReadonly            = 0x02,
        PageWriteCopy           = 0x08,
        PageExecute             = 0x10,
        PageExecuteRead         = 0x20,
        PageExecuteReadWrite    = 0x40,
        PageExecuteWriteCopy    = 0x80,
        PageGuard               = 0x100,
        PageNoCache             = 0x200,
        PageWriteComBine        = 0x400,
    }

    /// <summary>
    /// Kernel32 导出函数
    /// </summary>
    internal static class Kernel32
    {
        #region 方法
        /// <summary>
        /// 虚拟保护
        /// </summary>
        /// <param name="lpAddress"></param>
        /// <param name="dwSize"></param>
        /// <param name="flNewProtect"></param>
        /// <param name="lpflOldProtect"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "VirtualProtect", SetLastError = true)]
        internal static extern Boolean VirtualProtect(IntPtr lpAddress, UInt32 dwSize, Protection flNewProtect, out UInt32 lpflOldProtect);
        #endregion
    }
}
