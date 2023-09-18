using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PropertyReplicator
{
    public class CopyTool
    {
        // 这个静态方法接收两个类的泛型T和U，以及它们的实例t和u，直接复制它们的同名属性的值
        public static void CopyProperties<S, T>(S source, T targrt) where S : class where T : class
        {
            Type typeS = typeof(S);
            Type typeT = typeof(T);

            //命令式
            foreach (PropertyInfo propS in typeT.GetProperties())
            {
                // 获取属性的名称
                string propName = propS.Name;

                // 尝试在 T 中找到同名的属性
                PropertyInfo propT = typeT.GetProperty(propName);

                // 如果找到同名属性，且 S 中可读， T 中可写
                if (propT != null && propS.CanRead && propT.CanWrite)
                {
                    /*
                     * 将 S 中属性复制给 T 中属性
                     * 注：这里直接使用 GetValue() 和 SetValue() 取值赋值，因此不能操作私有成员。(可以使用反射操作，这里不赘述)
                     */
                    propT.SetValue(targrt, propT.GetValue(source));
                }
            }

            //LINQ查询式
            var query = from propS in typeS.GetProperties()
                        let propT = typeT.GetProperty(propS.Name)
                        where propT != null && propS.CanRead && propT.CanWrite
                        select new { propS, propT };
            foreach(var pair in query)
            {
                pair.propT.SetValue(targrt, pair.propS.GetValue(source));
            }

            //LINQ链式
            typeS.GetProperties()
                .Select(propS => new { propS, propT = typeT.GetProperty(propS.Name) })
                .Where(pair => pair.propT != null && pair.propS.CanRead && pair.propT.CanWrite)
                .ToList()
                .ForEach(item => item.propT.SetValue(targrt, item.propS.GetValue(source)));
        }
    }
}
