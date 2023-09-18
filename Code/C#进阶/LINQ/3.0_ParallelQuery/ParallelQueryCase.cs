/*
 * 使用 AsParallel() 实现多线程查询
 * 注：.AsParallel() 会将原本的 IEnumerable 返回值类型改成 ParallelQuery 返回值类型
 */
/*var arr1 = Enumerable
    .Range(1, 10)
    .ToArray()
    .AsParallel()
    .Select(x =>
    {
        Thread.Sleep(500);
        return x * x;
    })
    .OrderBy(x => x);       //多线程会将顺序打乱，因此需要排序

foreach (var n in arr1)
    Console.WriteLine(n);*/

//使用 .AsOrdered() 排序
var arr2 = Enumerable
    .Range(1, 10)
    .ToArray()
    .AsParallel()
    .AsOrdered()            // .AsParallel() 下更好的排序方法
    .Select(x =>
    {
        Thread.Sleep(500);
        return x * x;
    });

foreach (var n in arr2)
    Console.WriteLine(n);

//使用 .AsSequential() 回到单线程
var arr3 = Enumerable
    .Range(1, 10)
    .ToArray()
    .AsParallel()
    .AsOrdered()
    .Select(x =>
    {
        Thread.Sleep(500);
        return x * x;
    })
    .AsSequential();        // .AsSequential() 可以将返回值类型改回 IEnumerable ，并且回到单线程，以便后续操作

foreach (var n in arr3)
    Console.WriteLine(n);