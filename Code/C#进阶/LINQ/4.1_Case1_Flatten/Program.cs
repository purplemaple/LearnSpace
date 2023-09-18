/*//实战案例：展平
var mat = new int[][]
{
    new[]{1, 2, 3, 4 },
    new[]{5, 6, 7, 8 },
    new[]{8, 9, 10, 11, 12 }
};

*//*
 * 查询表达式：
 *//*
var res1 =
    from row in mat
    from n in row
    select n;

foreach (var n in res1)
    Console.WriteLine(n);

*//*
 * 链式表达式：没啥好说的，.SelectMany() 相当于查询表达式的多层 from，可以拿到每一项自己下面的每一小项
 * 例如：字符串数组的每一项是字符串，字符串的每一小项是字符
 *//*
var res2 = mat
    .SelectMany(x => x);

foreach (var n in res2)
    Console.WriteLine(n);*/

//实战案例：笛卡尔积
//打印一个 5 * 4 * 3 = 60 行，3 列的结果
//三列数字分别为从 (0 ~ 4)，(0 ~ 3)，(0 ~ 2)
/*
 * 传统嵌套循环法
 */
/*for(int i = 0; i < 5; i++)
{
    for(int j = 0; j < 4; j++)
    {
        for(int k = 0; k < 3; k++)
        {
            Console.WriteLine($"{i},{j},{k}");
        }
    }
}*/

/*
 * 查询法
 */
/*var prods1 =
    from i in Enumerable.Range(0, 5)
    from j in Enumerable.Range(0, 4)
    from k in Enumerable.Range(0, 3)
    select $"{i},{j},{k}";

foreach(var n in prods1)
    Console.WriteLine(n);*/

/*
 * 链式法：不如查询法简洁，仅供图一乐
 */
var prods2 = Enumerable
    .Range(0, 5)
    .SelectMany(r => Enumerable.Range(0, 4), (l, r) => (l, r))          // (l, r) => (l, r) 左边的是 Lambda 表达式的传参，右边是返回值，实际是把 l 与 r 拼到一起
    .SelectMany(r => Enumerable.Range(0, 3), (l, r) => (l.l, l.r, r))   // 三列：上轮结果的左边的左边；左边的右边；右边
    .Select(x => x.ToString());

foreach (var n in prods2)
    Console.WriteLine(n);