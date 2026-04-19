using FreeSql.DBCore.Entities;

string connStr = @"Data Source=DESKTOP-ETHSW\SQLEXPRESS;Initial Catalog=ZhaoxiFreeSqlDb;" +
    "Persist Security Info=True;User ID=sa;Password=test;Trust Server Certificate=True";

IFreeSql fsql = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.SqlServer, connStr)
        .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))  //监听SQL语句
        .UseAutoSyncStructure(false) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
        .Build();

//先拿到数据，方便后面测试删除操作
var companies = fsql.Queryable<Company>()
    .OrderByDescending(x => x.Id)
    .First();

/*
 * 0. 动态表名
 * 只对 tb_Test 表删除
 */
//fsql.Delete<Company>(1).AsTable("tb_Test").ExecuteAffrows(); 

/*
 * 1. 动态条件删除(将传入对象的删除)
 * 注：未使用 .Where 时，只会匹配主键，详见下方生成的 SQL 语句
 */

// Sql：DELETE FROM [Company] WHERE ([Id] = 64)
//var t1 = fsql.Delete<Company>(companies).ExecuteAffrows();

/*
 * 这里未指定主键 Id，即使指定了名字也没用
 * Sql：DELETE FROM [Company] WHERE ([Id] = 0)
 */
//var t2 = fsql.Delete<Company>(new Company { CompanyName = companies.CompanyName}).ExecuteAffrows();

/* 
 * 未指定主键的项生成的sql语句中 Id 为 0
 * Sql：DELETE FROM [Company] WHERE ([Id] IN (1,0))
 */
/*var t3 = fsql.Delete<Company>(new[] {
                    new Company { Id = 1, CompanyName = "test" },
                    new Company { CompanyName = "test" } }).ExecuteAffrows();*/


// Sql：DELETE FROM [Company] WHERE ([Id] = 1)
//var t4 = fsql.Delete<Company>(new { id = 1 }).ExecuteAffrows();

// 出于安全考虑，没有条件不执行删除动作，避免误删除全表数据
//var t5 = fsql.Delete<Company>().ExecuteAffrows();


/*
 * 2. 条件删除(只支持单条件)
 */

//Sql：DELETE FROM [Company] WHERE ([CompanyName] = N'Free.com1')
//var t6 = fsql.Delete<Company>().Where(a => a.CompanyName == "Free.com1").ExecuteAffrows();


//var t7 = fsql.Delete<Company>().Where(a => a.Id == 1).ExecuteAffrows();

//Sql：DELETE FROM [Company] WHERE (id = @id)
//var t8 = fsql.Delete<Company>().Where("id = @id", new { id = 1 }).ExecuteAffrows();

//Sql：DELETE FROM [Company] WHERE ([Id] = 1)
/*var item = new Company { Id = 1, CompanyName = "newtitle" };
var t89 = fsql.Delete<Company>().Where(item).ExecuteAffrows();*/

//Sql：DELETE FROM [Company] WHERE ([Id] IN (1,2,3,4,5,6,7,8,9,10))
/*var items = new List<Company>();
for (var a = 0; a < 10; a++) items.Add(new Company
{
    Id = a + 1,
    CompanyName = $"newtitle{a}",
    CreateTime = DateTime.Now
});
var t10 = fsql.Delete<Company>().Where(items).ExecuteAffrows();*/


/*
 * 3. 字典删除(多条件删除)
 * Sql：DELETE FROM [Company] WHERE ([Id] = 1 AND [CompanyName] = N'xxxx')
 */
/*var dic = new Dictionary<string, object>
{
    { "Id", 1 },
    { "CompanyName", "xxxx" }
};
fsql.DeleteDict(dic).AsTable("Company").ExecuteAffrows();*/


/*
 * 4. 高级删除 
 * 即先查询后使用 .ToDelete 将查询到的结果删除
 * Sql：DELETE FROM [Company] WHERE ([Id] in (select * from (SELECT a.[Id] FROM [Company] a WHERE (a.[Id] = 1)) ftb_del))
 */
fsql.Select<Company>()
    .Where(a => a.Id == 1)
    .ToDelete()
    .ExecuteAffrows();


/*
 * 5. 级联删除
 */

/*
 * 5.1 基于 [对象] 级联删除
 * TODO: 待补充
 */

/*
 * 5.2 基于 [数据库] 级联删除
 * TODO: 待补充
 */

Console.ReadKey();