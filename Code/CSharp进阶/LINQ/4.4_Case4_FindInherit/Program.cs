//案例4：寻找派生类
using System.Reflection;

//获取当前程序集中所有类
var types = Assembly
    .GetAssembly(typeof(Exception))!
    .GetTypes();

/*
 * 链式表达式
 */
/*var res1 = types 
    .Where(t => t.IsAssignableTo(typeof(Exception)))
    .Select(t => t.Name)
    .OrderBy(t => t.Split().Length);

foreach (var n in res1)
    Console.WriteLine(n);*/


/*
 * 查询表达式
 */
var res2 =
    from type in types
    where type.IsAssignableTo(typeof(Exception))
    select type.Name into name
    let wc = name.Split().Length
    group name by wc into g
    orderby g.Key
    select new { Count = g.Key, Items = g.ToList() };

foreach (var n in res2)
    Console.WriteLine(n);
