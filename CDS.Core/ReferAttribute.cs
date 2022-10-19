using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CDS.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ReferAttribute : Attribute
    {
        public string Key { get; }
        public Type Type { get; }

        public ReferAttribute(string key, Type type)
        {
            Key = key;
            Type = type;
        }
    }

    public static class ReferAttributeHelper
    {
        public static object? CreateReferInstance(this object obj, string key, Type[] types, params object[] para)
        {
            try
            {
                return obj.GetType().GetCustomAttributes<ReferAttribute>()
                    .Where(a => a.Key == key)
                    .FirstOrDefault()?.Type?.GetConstructor(types)?.Invoke(para);
            }
            catch
            {
                return null;
            }
        }

        public static object? CreateReferInstance(this object obj, string key)
            => CreateReferInstance(obj, key, new Type[] { obj.GetType() }, new object[] { obj });
    }
}
