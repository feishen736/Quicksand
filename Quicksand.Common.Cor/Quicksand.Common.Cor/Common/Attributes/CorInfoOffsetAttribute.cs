using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Cor.Common
{
    /// <summary>
    /// 地址偏移
    /// </summary>
    [AttributeUsage(AttributeTargets.Delegate, AllowMultiple = true)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal class CorInfoOffsetAttribute : Attribute
    {
        #region 属性
        /// <summary>
        /// 偏移量
        /// </summary>
        internal Int32 Offset { get; private set; }
        /// <summary>
        /// 版本号
        /// </summary>
        internal Version Version { get; private set; }
        #endregion

        #region 构造
        internal CorInfoOffsetAttribute(String version, Int32 offset32, Int32 offset64)
        {
            if (Environment.Is64BitProcess)
                this.Offset = offset64;
            else
                this.Offset = offset32;

            this.Version = new Version(version);
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取地址偏移
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static CorInfoOffsetAttribute GetCorInfoOffset<T>() where T : Delegate
        {
            var infos = typeof(T).GetCustomAttributes<CorInfoOffsetAttribute>();

            var value = infos.FirstOrDefault(s => s.Version < Clrjit.ClrjitVersion);

            if (value != null)
                return value;
            else
                throw new Exception("未找到指定版本文件!");
        }
        #endregion
    }
}
