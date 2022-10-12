using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.InstrumentModel
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
}
