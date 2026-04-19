using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GenericityReflectionFactory
{
    /// <summary>
    /// 泛型反射工厂<br/>
    /// 约束 T 必须是 class (类、抽象类、接口)
    /// </summary>
    /// <typeparam name="T">想要创建的类对象(一般传入基类或接口，输出子类或实现类)</typeparam>
    public class TReflectionFactory<T> where T : class
    {
        public Dictionary<object, Type> dict = new Dictionary<object, Type>();

        public TReflectionFactory()
        {
            //命令式
            //1. 获取当前程序集中所有类型
            /*Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {

                *//*2.遍历所有类型，判断是否继承(或实现)自 T, 并且判断是否被 FactoryAttribute 特性标记
                 *typeof(T).IsAssignableFrom(type)：                   检验 type 是否继承(或实现)自 T
                 *type.IsDefined(typeof(FactoryAttribute), false)：    检验 type 是否被 FactoryAttribute 特性标记，false：不追溯特性的派生类
                *//*

                if (typeof(T).IsAssignableFrom(type) && type.IsDefined(typeof(FactoryAttribute), false))
                {
                    //3. 获取筛选后的 type 上的 FactoryAttribute
                    FactoryAttribute attr = type.GetCustomAttribute<FactoryAttribute>();
                    //4. 将特性的 Key 作为字典的 key, 当前 type 作为字典的 value 存入字典，后续使用时可根据 Key 准确获取对应的实现类
                    dict.Add(attr.Key, type);
                }
            }*/

            //查询表达式
            /*var query = from type in Assembly.GetExecutingAssembly().GetTypes()
                        where typeof(T).IsAssignableFrom(type) && type.IsDefined(typeof(T), false)
                        select new {Key = type.GetCustomAttribute<FactoryAttribute>().Key , Type = type};
            foreach (var item in query)
            {
                dict.Add(item.Key, item.Type);
            }*/

            //链式表达式
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(T).IsAssignableFrom(type) && type.IsDefined(typeof(FactoryAttribute), false))
                .Select(type => new { Key = type.GetCustomAttribute<FactoryAttribute>().Key, Type = type })
                .ToList()
                .ForEach(item => dict.Add(item.Key, item.Type));
        }

        /// <summary>
        /// 创建实现类的方法
        /// </summary>
        /// <param name="key">特性的 Key </param>
        /// <param name="parameters">特性的 Parameters，用于调用构造函数时传入参数列表</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public T Create(object key, params object[] parameters)
        {
            //判断字典中是否存在 Key
            if (dict.ContainsKey(key))
            {
                Type type = dict[key];
                //使用反射构建对象，传递 parameters 作为构造函数的参数(parameters为null时表示调用无参构造)，然后用 as 转为 T 类型对象
                return Activator.CreateInstance(type, parameters) as T;
            }
            else
            {
                //字典中不存在 Key 时抛异常，可能是未找到标有指定特性的类，所以未添加到字典中导致的
                throw new ArgumentException("Key 不存在");
            }
        }
    }
}
