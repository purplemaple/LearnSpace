﻿//需求：从两个数组中求出交集

int[] arr1 = new int[] { 1, 2, 3, 4, 5, 6 };
int[] arr2 = new int[] { 4, 5, 6, 7, 8, 9 };


/*
 * 传统方法：
 */
List<int> res1 = new();
foreach(var n in arr1)
    if(arr2.Contains(n))
        res1.Add(n);

foreach(var n in res1)
    Console.WriteLine(n);


/*
 * 链式表达式：
 * 注：Intersect() 是链式表达式中求交集的扩展方法，查询表达式中没有
 */
var res2 = arr1.Intersect(arr2);

foreach (var n in res2)
    Console.WriteLine(n);

/*
 * 查询表达式：
 * 查询表达式的语法基本与 SQL 语句相同，所以出现 SQL 语句里面没有的东西时，查询表达式就不那么好用了
 * 例：求交集只能这样写：
 */
var res3 =
    from n in arr1
    where arr2.Contains(n)
    select n;

foreach (var n in res3)
    Console.WriteLine(n);
