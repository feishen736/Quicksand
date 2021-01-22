using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace Quicksand.Common.Aop.Common
{
    /// <summary>
    /// 成员拦截特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public abstract class MemberInterceptionAttribute : Attribute
    {
        #region 构造
        public MemberInterceptionAttribute()
        {
            this.Accessor = Accessor.Non;
        }
        public MemberInterceptionAttribute(Accessor accessor)
        {
            this.Accessor = accessor;
        }
        #endregion

        #region 属性
        public Accessor Accessor { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 进入方法
        /// </summary>
        /// <param name="ExecutionArgs">执行参数</param>
        public abstract void OnEntryMethod(ExecutionArgs args);
        /// <summary>
        /// 异常方法
        /// </summary>
        /// <param name="ExecutionArgs">执行参数</param>
        public abstract void OnErrorMethod(ExecutionArgs args);
        /// <summary>
        /// 退出方法
        /// </summary>
        /// <param name="ExecutionArgs">执行参数</param>
        public abstract void OnExitsMethod(ExecutionArgs args);

        /// <summary>
        /// 获取特性
        /// </summary>
        /// <param name="methodInfo">方法信息</param>
        /// <returns>特性集合</returns>
        internal static List<MemberInterceptionAttribute> GetMemberInterceptionAttributes(MethodBase methodInfo)
        {
            var memberInterception = new List<MemberInterceptionAttribute>();


            // 方法成员
            if (methodInfo.IsDefined(typeof(FilterInterceptionAttribute)))
                return memberInterception;
            if (methodInfo.IsDefined(typeof(MemberInterceptionAttribute)))
                memberInterception.AddRange(methodInfo.GetCustomAttributes<MemberInterceptionAttribute>().ToList());
            if (methodInfo.ReflectedType != null && methodInfo.ReflectedType.IsDefined(typeof(MemberInterceptionAttribute)))
                foreach (var item in methodInfo.ReflectedType.GetCustomAttributes<MemberInterceptionAttribute>())
                    if (item.Accessor == Accessor.Non)
                        memberInterception.Add(item);

            // 属性成员
            if (methodInfo.Name.Length <= 4)
                return memberInterception;
            if (methodInfo.Name.Substring(0, 4) != "get_" && methodInfo.Name.Substring(0, 4) != "set_")
                return memberInterception;

            var flages = BindingFlags.Default;

            if (methodInfo.IsPublic)
                flages |= BindingFlags.Public;
            else
                flages |= BindingFlags.NonPublic;

            if (methodInfo.IsStatic)
                flages |= BindingFlags.Static;
            else
                flages |= BindingFlags.Instance;

            var propertyRef = methodInfo.ReflectedType;
            var propertyName = methodInfo.Name.Substring(4);
            var propertyPrefix = methodInfo.Name.Substring(0, 4);
            var propertyInfo = propertyRef.GetProperty(propertyName, flages);

            if (propertyInfo == null)
                return memberInterception;
            if (Attribute.IsDefined(propertyInfo, typeof(FilterInterceptionAttribute)))
                return memberInterception;

            if (Attribute.IsDefined(propertyInfo, typeof(MemberInterceptionAttribute)))
            {
                foreach (var item in propertyInfo.GetCustomAttributes<MemberInterceptionAttribute>())
                {
                    if (item.Accessor == Accessor.Non)
                        continue;
                    else if (item.Accessor == Accessor.All)
                        memberInterception.Add(item);
                    else if (item.Accessor == Accessor.Get && propertyPrefix == "get_")
                        memberInterception.Add(item);
                    else if (item.Accessor == Accessor.Set && propertyPrefix == "set_")
                        memberInterception.Add(item);
                    else
                        continue;
                }
            }
            if (Attribute.IsDefined(propertyRef, typeof(MemberInterceptionAttribute)))
            {
                foreach (var item in propertyRef.GetCustomAttributes<MemberInterceptionAttribute>())
                {
                    if (item.Accessor == Accessor.Non)
                        continue;
                    else if (item.Accessor == Accessor.Get && propertyPrefix == "get_")
                        memberInterception.Add(item);
                    else if (item.Accessor == Accessor.Set && propertyPrefix == "set_")
                        memberInterception.Add(item);
                    else
                        continue;
                }
            }

            return memberInterception; 
        }
        #endregion
    }
}
