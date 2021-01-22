using Quicksand.Common.Cor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Aop.Common
{
    /// <summary>
    /// MethodBase 方法扩展
    /// </summary>
    public static class MethodBaseExtension
    {
        /// <summary>
        /// 获取拦截特性
        /// </summary>
        /// <param name="methodBase"></param>
        /// <returns>特性信息</returns>
        public static List<MemberInterceptionAttribute> GetInterceptionAttributes(this MethodBase methodBase)
        {
            return MemberInterceptionAttribute.GetMemberInterceptionAttributes(methodBase);
        }
    }
}
