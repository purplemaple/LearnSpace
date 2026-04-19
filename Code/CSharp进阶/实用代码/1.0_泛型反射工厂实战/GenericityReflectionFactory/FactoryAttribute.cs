using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericityReflectionFactory
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FactoryAttribute : Attribute
    {   
        /*
         * 注：C# 11 后可以使用带泛型的特性，这里演示不可使用的情况，属性类型都使用 object 代替
         */
        public object Key { get; set; }
        public object[] Parameters { get; set; }

        public FactoryAttribute(object key, params object[] Parameters)
        {
            this.Key = key;
            this.Parameters = Parameters;
        }
    }
}
