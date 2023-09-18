//需求： 筛选出所有偶数 && >= 4 然后排序

List<int> lst = new() { 1, 3, 5, 7, 9, 2, 4, 6, 8, 0 };

/*
 * 命令式编程：
 */
/*List<int> res1 = new();

foreach(var n in lst)
    if(n % 2 == 0 && n >= 4)
        res1.Add(n);

res1.Sort();
foreach(var n in res1)
    Console.WriteLine(n);*/

/*
 * 查询表达式：(query ecpression)
 */
var res2 = 
    from n in lst
    where n % 2 == 0 && n >= 4
    orderby n
    select n;

foreach (var n in res2)
    Console.WriteLine(n);

/*
 * 链式表达式：(chained expression)
 * 注：链式表达式全都是靠 . 连接，实际上只有一句，一直都是对最开始的集合进行操作，所以最终返回的都是可枚举类型(IEunmable)，所以不需要用 select 来表示语句的完结(非要加上也可以)
 */
var res3 = lst
    .Where(n => n % 2 == 0 && n >= 4)
    .OrderBy(n => n);

foreach (var n in res3)
    Console.WriteLine(n);