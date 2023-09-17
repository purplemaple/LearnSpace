//需求：从长度为200的数组中统计数字频率  (给定了随机种子，所以每次数组都一样；数组元素从0 ~ 20取值)

Random rnd = new(1334);
var arr = Enumerable.Range(0, 200).Select(_ => rnd.Next(20));

/*
 * 传统方法：
 */
/*Dictionary<int, int> dic = new();
foreach (var n in arr)
{
    if(dic.TryGetValue(n, out int count))
        dic[n] = count + 1;
    else
        dic[n] = 1;
}
foreach (var n in dic)
    Console.WriteLine(n);*/

/*
 * 链式表达式：
 * 思想：把所有数据按自己的值分组，相等的分到一组，最后统计每组的个数就行了
 * 如数字 2 就是这样放的：<Kay = 2，Value = 2,2,2,2......> 一共有多少个 2，Value 里就放多少个，最后统计 Value 个数就等于数字 2 出现的频率了
 */
var res1 = arr
    .GroupBy(x => x)
    .ToDictionary(g => g.Key, g => g.Count());

/*
 * 注：可以只用匿名类代替 ToDictionary()
 * 这里的匿名类：含 Key 和 Count 两个属性，因为在 g 中的 Key 本来就是属性，所以匿名类里不用取名，而 Count 是 g 调用计算数量的方法得出的结果，因此需要先命名
 */
var res2 = arr
    .GroupBy(x => x)
    .Select(g => new { g.Key, Count = g.Count() });

foreach (var n in res2)
    Console.WriteLine(n);

/*
 * 查询表达式:
 * 思想：把所有数据按自己的值分组，相等的分到一组，最后统计每组的个数就行了
 * 如数字 2 就是这样放的：<Kay = 2，Value = 2,2,2,2......> 一共有多少个 2，Value 里就放多少个，最后统计 Value 个数就等于数字 2 出现的频率了
 * 
 * 注：这里使用 group 将 x 分组后，输出结果是一堆 Grouping 类型(其实就是键值对)，
 * 我们希望分别操作每一组 Grouping，而不希望操作散装的一堆 Grouping，因此使用 into
 * 这里使用 into 给分组后的所有组取了个别名叫 g ，然后 select 操作 g 就可以取到每一个 g 里面的 Key 和 Value 了
 */
var res3 =
    from x in arr
    group x by x into g
    select new { g.Key, Count = g.Count() };

foreach (var n in res3)
    Console.WriteLine(n);

/*
 * 链式表达式和查询表达式的选择：
 * 各有优劣，如：链式中独有 Intersect() 方法，而查询表达式中独有 let 语法
 *      链式表达式：
 *          求最大值、最小值、数量、求和、随机查找(如找出前几项、最后几项等)
 *      
 *      查询表达式：
 *          筛选、排序、合并等操作时
 */