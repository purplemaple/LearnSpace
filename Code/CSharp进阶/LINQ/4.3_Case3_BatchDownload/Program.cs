//案例3：批量下载文件
using System.Threading.Tasks;

var urls = new string[]
{
    "http://www.example.com/pic1.jpg",
    "http://www.example.com/pic2.jpg",
    "http://www.example.com/pic3.jpg",
};

/*
 * 传统方法：
 */
/*List<Task> tasks1 = new();
foreach(var url in urls)
{
    tasks.Add(DownloadAsync(url, url.Split('/').Last()));
}
await Task.WhenAll(tasks1);*/

/*
 * 链式表达式：
 */
var tasks2 = urls
    .Select(url => DownloadAsync(url, url.Split('/').Last()));

await Task.WhenAll(tasks2);     //注：使用 LINQ 语法输出的 tasks 类型是 IEnumerable ，这里直接往 WhenAll() 里面放是一种比较新的语法，老版本可能没有

/*
 * 查询表达式
 */
/*var tasks3 =
    from url in urls
    let filename = url.Split('/').Last()    //拿到文件名
    where filename != "pic2.jpg"            //根据文件名筛选一下，图一乐
    select DownloadAsync(url, filename);

await Task.WhenAll(tasks3);     //注：使用 LINQ 语法输出的 tasks 类型是 IEnumerable ，这里直接往 WhenAll() 里面放是一种比较新的语法，老版本可能没有*/

/// 下载方法
async Task DownloadAsync(string url, string filename)
{
    await Task.Delay(1000); //模拟下载延时，每次延时 1 秒
    Console.WriteLine($"{filename} download.");
}