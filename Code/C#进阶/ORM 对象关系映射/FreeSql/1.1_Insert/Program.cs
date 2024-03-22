using FreeSql.DBCore.Entities;

string connStr = @"Data Source=DESKTOP-ETHSW\SQLEXPRESS;Initial Catalog=ZhaoxiFreeSqlDb;" +
    "Persist Security Info=True;User ID=sa;Password=test;Trust Server Certificate=True";

IFreeSql fsql = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.SqlServer, connStr)
        .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))  //监听SQL语句
        .UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
        .Build();

List<Company> companies = new();
for (int i = 0; i < 10; i++)
{
    companies.Add(new Company() { CompanyName = $"Free.com" + i.ToString(), CreateTime = DateTime.Now });
}

/* 
 * 1. 单条插入
 * 返回受影响的行数
 * Sql：INSERT INTO [Company]([CompanyName], [CreateTime]) VALUES(@CompanyName_0, @CreateTime_0)
 */
/*var t1 = fsql.Insert(companies[0]).ExecuteAffrows();*/

/* 
 * 1.1
 * 返回 id
 * Sql：INSERT INTO [Company]([CompanyName], [CreateTime]) VALUES(@CompanyName_0, @CreateTime_0); SELECT SCOPE_IDENTITY();
 */
/*long id = fsql.Insert(companies[0]).ExecuteIdentity();
companies[0].Id = (int)id;*/

/* 
 * 1.2
 * 返回插入成功的那个对象
 * 因为可能涉及到批量插入，所以返回类型是集合
 * Sql：INSERT INTO [Company]([CompanyName], [CreateTime]) OUTPUT INSERTED.[Id] as [Id],
 * INSERTED.[CompanyName] as [CompanyName], INSERTED.[CreateTime] as [CreateTime] 
 * VALUES(@CompanyName_0, @CreateTime_0)
 */
/*List<Company>? result = fsql.Insert(companies[0]).ExecuteInserted();*/

// 2. 批量插入
/* 
 * 2.1 一般方法(低性能)
 * Sql：INSERT INTO [Company]([CompanyName], [CreateTime]) 
 * VALUES(@CompanyName_0, @CreateTime_0), (@CompanyName_1, @CreateTime_1), (@CompanyName_2, @CreateTime_2),
 * (@CompanyName_3, @CreateTime_3), (@CompanyName_4, @CreateTime_4), (@CompanyName_5, @CreateTime_5),
 * (@CompanyName_6, @CreateTime_6), (@CompanyName_7, @CreateTime_7), (@CompanyName_8, @CreateTime_8),
 * (@CompanyName_9, @CreateTime_9)
 */
/*var t2 = fsql.Insert(companies).ExecuteAffrows();

var t3 = fsql.Insert(companies).ExecuteInserted();*/


/* 
 * 2.2 BulkCopy 高性能批量插入(无返回值)
 */
/*List<Company> insertList = new List<Company>();
for (int i = 0; i < 100000; i++)
{
    Company addCompany = new Company()
    {
        CompanyName = "测试批量插入数据",
        CreateTime = DateTime.Now
    };
    insertList.Add(addCompany);
}

//普通批量插入方法
Stopwatch stopwatch1 = Stopwatch.StartNew();
int fResult = fsql.Insert<Company>(insertList).ExecuteAffrows();
stopwatch1.Stop();
Console.WriteLine($"10W条数据批量操作，普通方式耗时：{stopwatch1.ElapsedMilliseconds}ms");

//BulkCopy 批量插入方法(无返回值)
Stopwatch stopwatch2 = Stopwatch.StartNew();
fsql.Insert<Company>(insertList).ExecuteSqlBulkCopy();
stopwatch2.Stop();
Console.WriteLine($"10W条数据批量操作，ExecuteSqlBulkCopy操作方式耗时：{stopwatch2.ElapsedMilliseconds}ms");*/


/* 
 * 3. 动态表名
 * 对 "Test" 表插入
 * 注：指定一个不存在的表时，如果处于 CodeFirst 模式下(.UseAutoSyncStructure(true))，则会先创建该表后再进行插入。否则报错
 */
/*fsql.Insert(companies).AsTable("Test").ExecuteAffrows();*/

// 4. 列插入
/* 
 * 4.1 插入指定的列
 * 其他列为 null
 * TODO: 如果其他列不可为 null 时，会怎样？
 */
/*var t4 = fsql.Insert(companies).InsertColumns(a => a.CreateTime).ExecuteAffrows();

var t5 = fsql.Insert(companies).InsertColumns(a => new {a.CompanyName, a.CreateTime}).ExecuteAffrows();*/

/* 
 * 4.2 忽略列
 */
/*var t6 = fsql.Insert(companies).IgnoreColumns(a => a.CreateTime).ExecuteAffrows();

var t7 = fsql.Insert(companies).IgnoreColumns(a => new { a.CompanyName, a.CreateTime }).ExecuteAffrows();*/

/*
 * 4.3 列插入时的优先级
 * 全部列 < 指定列(InsertColumns) < 忽略列(IgnoreColumns)
 */

/* 
 * 5. 字典插入
 * 注：
 *      1. 需指定插入到哪个表，不管是 CodeFirst 还是 DBFirst，表名不匹配都会报错
 *      2. 字典的 key 需要和列名匹配
 */
/*var dic = new Dictionary<string, object>
{
    { "CompanyName", "FreeSql.com 字典插入" },
    { "CreateTime", DateTime.Now }
};

fsql.InsertDict(dic).AsTable("Company").ExecuteAffrows();*/

/* 
 * 6. 导入表数据
 * 从某个地方查询了一系列数据后，再导入到表中
 */
int affrows = fsql.Select<Company>()
  .Limit(10)
  .OrderBy(a => a.Id)
  .InsertInto(null, a => new Company
  {
      CompanyName = a.CompanyName + "_New"
  });

Console.ReadKey();