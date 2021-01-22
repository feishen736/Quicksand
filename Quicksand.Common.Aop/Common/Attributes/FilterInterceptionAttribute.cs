using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Quicksand.Common.Aop.Common
{
    /// <summary>
    /// 过滤拦截特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class FilterInterceptionAttribute : Attribute
    {
    }
}
