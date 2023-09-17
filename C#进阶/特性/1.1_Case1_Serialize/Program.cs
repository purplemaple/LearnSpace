using System.Reflection;

// 特性 Attribute
// 前置课程: 泛型、反射、LINQ
Student student = new()
{
    Id = 13,
    Name = "Tom",
    Age = 17,
    Gender = Gender.Male,
    Class = "3A"
};


Serialize(student);
Console.WriteLine(Serialize(student));

string Serialize(object obj)
{
    var res = obj       //要注意：这里不能用 object 接收，因为结果是 IEnumerable<string> 类型对象(该对象重写了ToString()方法)，因此打印时可以得到它的内容，
        .GetType()                                   //而 object 类型对象没有实现 ToString() 方法，所以打印得到的是类名，而非内容
        .GetProperties(BindingFlags.Instance | BindingFlags.Public)     //拿到实例化的 Public 对象
        .Where(pi =>
        {
            BrowsableAttribute? attr = pi.GetCustomAttribute<BrowsableAttribute>();
            if (attr != null)
            {
                return attr.Browsable;      //如果特性标注为 false，则返回 false
            }
            return true;
        })
        .Select(pi => new { Key = pi.Name, Value = pi.GetValue(obj) })  //使用了匿名类
        .Select(o => $"{o.Key}: {o.Value}");

    return string.Join(Environment.NewLine, res);
}

class Student
{
    [Browsable(false)]
    public int Id { get; set; }

    public string Name { get; set; }

    /*
     * 注：同时给特性里的多个属性赋值
     * 这里 Browsable 在构造函数中有，所以直接写个 true 就可以按构造函数的参数顺序赋值
     * 而 Tag 不在构造函数中，所以用 Tag = "123" 这种方式赋值 (此语法仅适用于 Attribute )
     * 
     * 扩展：其他的如方法或普通类，在给可选参数或者非构造属性赋值时可用的语法：
     *      1. 方法： void Foo(int value, bool flag = false, string tag = ""){}        //这里 flag、tag 参数可以不传，默认是 false、""
     *         调用 Foo 方法时：Foo(100, tag: "123")
     *         
     *      2. 普通类：new Demo(100){ Tag = "123" };                                    //这里 100 是构造中赋值，Tag = "123" 是给构造中不含属性赋值
     */
    [Browsable(true, Tag = "123")]          
    public int Age { get; set; }

    public Gender Gender { get; set; }

    public string Class { get; set; }
}

enum Gender { Male, Female}

/*
 * AttributeUsage()：本身也是个特性，用于限制用户自定义特性的使用范围
 * AttributeTargets.Property：只能用于属性上
 * AllowMultiple = false：同名特性不能重复。(这里相当于限制一个属性上只能有一个[Browsable])
 */
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
class BrowsableAttribute : Attribute
{
    public bool Browsable { get; set; }

    public string Tag { get; set; } //这里特意不把这个属性放在构造里，用于演示不通过构造函数赋值

    public BrowsableAttribute(bool b)
    {
        this.Browsable = b;
    }
}