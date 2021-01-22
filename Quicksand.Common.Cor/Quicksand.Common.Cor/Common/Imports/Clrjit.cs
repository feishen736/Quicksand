using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    /// <summary>
    /// Clrjit.dll 导出函数
    /// </summary>
    internal static class Clrjit
    {
        #region 属性
        /// <summary>
        /// 版本信息
        /// </summary>
        internal static Version ClrjitVersion { get; private set; }
        #endregion

        #region 构造
        static Clrjit()
        {
            var path = String.Format(@"{0}Clrjit.dll", RuntimeEnvironment.GetRuntimeDirectory());

            var info = FileVersionInfo.GetVersionInfo(path).ProductVersion;

            Clrjit.ClrjitVersion = new Version(info);
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取引擎
        /// </summary>
        /// <returns></returns>
        [DllImport("Clrjit.dll", EntryPoint = "getJit", PreserveSig = true)]
        internal static extern IntPtr GetJit();
        #endregion
    }
}
