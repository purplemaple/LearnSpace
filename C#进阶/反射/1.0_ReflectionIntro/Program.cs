using System.Reflection;
using SqlServerDB;

namespace _1._0_ReflectionIntro
{
    /// <summary>
    /// 泛型: 代码重用
    /// 反射: 就是操作dll文件的一个类库
    /// 怎么使用:
    ///     1. 查找DLL文件
    ///     2. 通过Reflection反射类库里的各种方法来操作dll文件
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            //1. 加载DLL文件
            /*
             * 方式一:
             *      这个DLL文件要在启动项目的目录下
             */
            //Assembly assembly1 = Assembly.Load("SqlServerDB");

            /*
             * 方式二:
             *      输入完整路径
             */
            //Assembly assembly2 = Assembly.LoadFile("E:\\Code\\C# for VS_Code\\C#进阶\\反射\\SqlServerDB\\bin\\Debug\\net7.0\\SqlServerDB.dll");

            /*
             * 方式三:
             *      兼容前两种
             */
            Assembly assembly3 = Assembly.LoadFrom("SqlServerDB.dll");    //注：此时需要后缀名
            Assembly assembly4 = Assembly.LoadFrom("E:\\Code\\C# for VS_Code\\C#进阶\\反射\\SqlServerDB\\bin\\Debug\\net7.0\\SqlServerDB.dll");

            //2. 获取指定类型
            //程序集中有很多类，因此需要指定到底想操作哪个文件的哪个类
            /*
             * 2.1 根据类的名称直接指定
             */
            Type type = assembly4.GetType("SqlServerDB.ReflectionTest");

            /*
             * 2.2 循环遍历程序集中的所有类(泛型类除外)
             */
            foreach(var item in assembly4.GetTypes())
            {
                //然后这里可以操作筛选 item，得到指定的类，如下：
                if(typeof(ReflectionTest).IsAssignableFrom(item) && !item.IsInterface)  //item 是 ReflectionTest 类或其子类(实现类) && item 非接口
                {

                }
            }

            /*
             * 2.* 补充：
             *      - 获取构造函数
             *      - 获取构造函数的参数
             */
            foreach(var ctor in type.GetConstructors())     //获取 private 构造方法时需要传入 BindingFlags 不再赘述...
            {
                Console.WriteLine("构造方法：" + ctor);
                foreach (var param in ctor.GetParameters())
                {
                    Console.WriteLine("构造方法的参数：" + param);
                }
            }

            //3. 实例化
            ReflectionTest reflectionTest = new ReflectionTest();   //这种实例化是知道具体类型，并且只能调用 public 方法      -->     静态

            Console.WriteLine("---------------------------- Activator.CreateInstance() 方法实例化对象 ----------------------------");
            //3.1 使用 Activator 类的 CreateInstance() 方法实例化对象
            /*
             * 3.1.1 调用 public 无参构造实例化
             */
            object test1 = Activator.CreateInstance(type);
            var test2 = Activator.CreateInstance(type);
            ReflectionTest test3 = Activator.CreateInstance(type) as ReflectionTest;    //  这里知道具体类型，因此做了转换
            ReflectionTest test4 = (ReflectionTest)Activator.CreateInstance(type);      //  这里知道具体类型，因此做了转换

            /*
             * 3.1.2 调用 public 有参构造实例化
             *      注：如果参数数组无数据，或者为null，则调用无参构造
             */
            var test5 = Activator.CreateInstance(type, new object[] {"若幸"});
            var test6 = Activator.CreateInstance(type, new object[] {});
            var test7 = Activator.CreateInstance(type, null);

            /*
             * 3.1.3 调用 private 无参构造实例化
             */
            Type privateType = assembly4.GetType("SqlServerDB.ReflectionPrivateTest");
            var privateInstance1 = Activator.CreateInstance(privateType, true);

            /*
             * 3.1.4 调用 private 有参构造实例化
             */
            /*
             * 注：采用 Activator.CreateInstance() 调用 private 有参构造函数时，需要使用这个重载:
             * 
             *      Activator.CreateInstance(Type , BindingFlags, Binder? , object[]? , CultureInfo)
             *          Type：           目标类
             *          BindingFlags：   用于控制绑定和调用方式的标志，例如指定是否忽略大小写、是否调用静态成员、是否调用私有成员等
             *          Binder：         一个Binder对象，用于解析方法重载和执行类型转换。如果为null，则使用默认的Binder对象
             *          object[]：       参数数组，如果为null则调用无参构造，如果数组元素为null(如：new object[] {null. null})，则调用有参构造，null也是一个参数，几个参数就调几个参数的构造
             *          CultureInfo：    一个CultureInfo对象，用于控制区域性特定的格式设置。如果为null，则使用当前线程的CultureInfo对象
             */
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var privateInstance2 = Activator.CreateInstance(privateType, flags, null, new object[] {"若幸"}, null);

            Console.WriteLine("---------------------------- Type.GetConstructor() 方法实例化对象 ----------------------------");

            //3.2 采用 Type 类的 GetConstructor() 方法实例化对象
            /*
             * 3.2.1 调用 public 无参构造实例化
             */
            ConstructorInfo ci1 = type.GetConstructor(new Type[] { });      //注意这里即使是无参，也必须传空数组 new Type[] { } ，不能传 null
            object obj1 = ci1.Invoke(null);

            /*
             * 3.2.2 调用 public 有参构造实例化
             */
            ConstructorInfo ci2 = type.GetConstructor(new Type[] { typeof(string) });   //new Type[] { } 数组中存放与参数列表一一对应的参数的类型
            object obj2 = ci2.Invoke(new object[] {"若幸"});                             //new object[] { } 数组存放与参数列表一一对应的参数的数据

            /*
             * 3.2.3 调用 private 无参构造实例化
             */
            BindingFlags flags2 = BindingFlags.Instance | BindingFlags.NonPublic;
            ConstructorInfo ci3 = privateType.GetConstructor(flags, new Type[] { });
            object obj3 = ci3.Invoke(null);

            /*
             * 3.2.4 调用 private 有参构造实例化
             */
            ConstructorInfo ci4 = privateType.GetConstructor(flags, new Type[] { typeof(string) });
            object obj4 = ci4.Invoke(new object[] { "若幸" });


            //4. 调用成员
            Console.WriteLine("---------------------------- 调用非泛型成员 ----------------------------");
            /*
             * 4.1 调用非泛型成员
             */
            ReflectionPrivateTest rt = obj4 as ReflectionPrivateTest;
            MethodInfo methodInfo1 = privateType.GetMethod("PrivateShow", BindingFlags.Instance | BindingFlags.NonPublic);  //第一个参数为方法名
            /*
             * 注：
             *      1. 这里第一个参数是成员所在的类, 这里既可以传转换前的 obj4，也可以传转换后的 rt
             *      2. 这里不管方法有没有参数，都必须传入参数数组
             * 
             */
            methodInfo1.Invoke(obj4, null);
            methodInfo1.Invoke(rt, null);  //如果方法有返回值，则用 object 接收即可

            Console.WriteLine("---------------------------- 调用泛型成员 ----------------------------");
            
            //4.2 调用泛型成员
            /*
             * 4.2.1 调用无参泛型方法
             */
            BindingFlags flags3 = BindingFlags.Instance | BindingFlags.NonPublic;
            //MethodInfo methodInfo2 = privateType.GetMethod("TShow1", flags3);                         //根据方法名拿到方法
            //MethodInfo genericMethodInfo = methodInfo2.MakeGenericMethod(new Type[] { });     //因为是泛型，所以还需设置泛型参数(T)的类型
            /*
             * 注：
             *      1. 上面两句可以合成一句
             *      2. MakeGenericMethod(new Type[] { typeof(string) }) 中传入的数组是泛型参数(T)的类型， 
             *                                                          此句等同于(如果方法是 public 的话): rt.TShow1<string>();
             *                                                          不要和上面调用构造函数实例化对象的 type.GetConstructor(new Type[] { typeof(string) }) 搞混了
             */
            MethodInfo methodInfo2 = privateType.GetMethod("TShow1", flags3).MakeGenericMethod(new Type[] { typeof(string) });
            //methodInfo2.Invoke(obj4, null);
            methodInfo2.Invoke(rt, null);   //这里传入的数组是方法的参数列表

            /*
             * 4.2.2 调用有参泛型方法
             */
            MethodInfo methodInfo3 = privateType.GetMethod("TShow2", flags3).MakeGenericMethod(new Type[] { typeof(int) });
            methodInfo3.Invoke(rt, new object[] {"若幸" });
            //注： 存在解析重载问题，解决方法如下：
            //获取方法时，指定方法参数的类型，类似于上面调用构造函数实例化对象的 type.GetConstructor(new Type[] { typeof(string) })
            MethodInfo methodInfo4 = privateType.GetMethod("TShow3", flags3, new Type[] { typeof(int), typeof(string) }).MakeGenericMethod(new Type[] { typeof(int) });
            MethodInfo methodInfo5 = privateType.GetMethod("TShow3", flags3, new Type[] { typeof(object), typeof(string) }).MakeGenericMethod(new Type[] { typeof(int) });
            
            methodInfo4.Invoke(rt, new object[] { 12, "若幸" });
            methodInfo5.Invoke(rt, new object[] { 12, "若幸" });

            Console.WriteLine("---------------------------- 操作属性、字段 ----------------------------");
            //5. 操作属性、字段
            //操作属性  -->  因为属性默认是 public 因此这里不需要用 BindingFlags 标识
            foreach (var property in privateType.GetProperties())
            {
                //设置属性
                if (property.Name.Equals("Name"))
                {
                    property.SetValue(rt, "小白");
                }
                else if (property.Name.Equals("Phone"))
                {
                    property.SetValue(rt, "134123456789");
                }

                //循环中顺便获取属性
                Console.WriteLine(property.GetValue(rt));
            }
            //获取属性  
            Console.WriteLine(privateType.GetProperty("Phone").GetValue(rt));

            //操作字段  --> 字段一般是 private 因此需要用 BindingFlags 标识
            BindingFlags flags4 = BindingFlags.Instance | BindingFlags.NonPublic;
            foreach (var field in privateType.GetFields(flags4))
            {
                //设置字段
                if (field.Name.Equals("count"))
                {
                    field.SetValue(rt, 2);
                }
                else if (field.Name.Equals("gender"))
                {
                    field.SetValue(rt, '女');
                }
            }
            Console.WriteLine(privateType.GetField("gender", flags4).GetValue(rt));


            /*
             * 附录0：
             *      privateType.GetMembers()：    获取所有成员
             *      privateType.GetEvent()：      获取指定事件
             *      privateType.GetInterface()：  获取指定接口
             *      privateType.GetEnumName()：   获取枚举类里指定 value 的名称
             *      privateType.GetEnumValues()： 获取枚举类里的所有 value
             *      ......
             */

            /*
             * 附录1：使用 Type 类的 GetConstructor() 方法， 或者 Activator 类的 CreateInstance() 方法构造对象的区别：
             * 
             *      1. GetConstructor：当目标类有多种构造函数时，可以更精准地控制调用哪个构造函数
             *         CreateInstance：会根据传入的参数自动匹配最合适的构造函数，但有可能出现重载解析问题，如：
             *              - 出现两个数量相同的构造函数：ctor(int a, string str){}  ctor(object obj, int i) 时，如果传入的参数数组是 {null, null} 时，报错
             *              - 对于这种情况，除了使用 GetConstructor 外，还可以自定义一个 Binder 对象放入参数中，来解析重载
             *         
             *      2. GetConstructor：需要先获取 ConstructorInfo 对象，然后再调用Invoke方法来创建实例，
             *         CreateInstance：可以直接创建实例
             * 
             *      3. GetConstructor：效率更高，因为可以直接获取指定类型的指定构造函数的 ConstructorInfo 对象，并缓存起来以供重复使用
             *         CreateInstance：效率更低，需要每次都根据传入的参数来匹配最合适的构造函数
             * 
             *      4. GetConstructor：不能使用激活属性,
             *         CreateInstance：可以使用激活属性（activation attributes）来传递一些额外的信息给远程对象
             */

            /*
             * 附录2：| 和 || 的区别：
             *      
             *      1. | ：按位或，操作整数类型(枚举内部有整型序号), 如：假设 BindingFlags.Instance 的值是 00000001，
             *                                                         BindingFlags.NonPublic 的值是 00000010，
             *                                                     那么 BindingFlags.Instance | BindingFlags.NonPublic 的值就是 00000011 
             * 
             *      2. || ：条件或，操作布尔类型，当第一个操作数为 true 时，结果已确定为 true ，因此会跳过第二个操作数的求值(短路行为)
             */

            /*
             * 附录3：除了 Type.GetConstructor() 和 Activator.CreateInstance() 构造对象外，还有以下几种方式：
             * 
             *      1. Assembly 类的 CreateInstance 方法，传入类型名称和可选的参数数组，来创建指定程序集中指定类型的实例
             * 
             *      2. FormatterServices 类的 GetUninitializedObject 方法，传入类型对象，来创建指定类型的未初始化的实例。这种方式不会调用任何构造函数，因此可以避免一些初始化逻辑。
             *      
             *      3. Expression 类的 New 方法，传入类型对象或构造函数表达式，来创建指定类型或表达式表示的类型的实例。这种方式可以利用表达式树来动态生成代码。
             */
        }
    }
}