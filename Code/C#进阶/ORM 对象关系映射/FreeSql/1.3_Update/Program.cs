using FreeSql.DBCore.Entities;

string connStr = @"Data Source=DESKTOP-ETHSW\SQLEXPRESS;Initial Catalog=ZhaoxiFreeSqlDb;" +
    "Persist Security Info=True;User ID=sa;Password=test;Trust Server Certificate=True";

IFreeSql fsql = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.SqlServer, connStr)
        .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))  //监听SQL语句
        .UseAutoSyncStructure(false) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
        .Build();

Company company1 = fsql.Queryable<Company>().First();
Company company2 = fsql.Queryable<Company>().OrderByDescending(x => x.Id).First();

/*
 * 0. 动态表名
 * 只对 tb_Test 表更新
 */
//fsql.Update<Company>(1).AsTable("tb_Test").ExecuteAffrows();


/*
 * 1. 动态条件更新
 * 动态条件支持的形式:
 *      1. 主键值
 *      2. 主键值数组：new[] { 主键值1, 主键值2 }
 *      3. 对象
 *      4. 对象数组：new[] { Topic对象1, Topic对象2 }
 *      5. new 对象：new { id = 1 }
 */
//Sql：UPDATE [Company] SET [CompanyName] = @p_0 WHERE ([Id] = 1)
fsql.Update<Company>(1)
    .Set(x => x.CompanyName, "FreeSql.com Update")
    .ExecuteAffrows();

//Sql：UPDATE [Company] SET [CompanyName] = @p_0 WHERE([Id] IN(1, 2))
fsql.Update<Company>(new[] { 1, 2 })
    .Set(x => x.CompanyName, "FreeSql.com Update")
    .ExecuteAffrows();

//Sql：UPDATE [Company] SET [CompanyName] = @p_0 WHERE([Id] = 11)
fsql.Update<Company>(company1)
    .Set(x => x.CompanyName, "FreeSql.com Update")
    .ExecuteAffrows();

//Sql：UPDATE [Company] SET [CompanyName] = @p_0 WHERE([Id] IN(11, 11))
fsql.Update<Company>(new[] { company1, company1 })
    .Set(x => x.CompanyName, "FreeSql.com Update")
    .ExecuteAffrows();

//Sql：UPDATE [Company] SET [CompanyName] = @p_0 WHERE([Id] = 1)
fsql.Update<Company>(new[] { 1 })
    .Set(x => x.CompanyName, "FreeSql.com Update")
    .ExecuteAffrows();

Console.ReadKey();