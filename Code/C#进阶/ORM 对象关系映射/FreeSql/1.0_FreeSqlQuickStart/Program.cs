using FreeSql.DBCore.Entities;

try
{
    IFreeSql fsql = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.SqlServer, @"Data Source=DESKTOP-ETHSW\SQLEXPRESS;Initial Catalog=ZhaoxiFreeSqlDb;Persist Security Info=True;User ID=sa;Password=test;Trust Server Certificate=True")
        .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))  //监听SQL语句
        .UseAutoSyncStructure(false) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
        .Build();

    Company addCompany = new()
    {
        CompanyName = "FreeSql.net",
        CreateTime = DateTime.Now,
    };

    /* 
     * 插入
     * Sql：INSERT INTO[Company]([CompanyName], [CreateTime]) VALUES(@CompanyName_0, @CreateTime_0)
     */
    int insertAffrows = fsql
        .Insert<Company>(addCompany)
        .AppendData(addCompany)
        .ExecuteAffrows();

    /* 
     * 查询
     * Sql：SELECT TOP 1 a.[Id], a.[CompanyName], a.[CreateTime]
     *      FROM [Company] a
     *      ORDER BY a.[Id] DESC
     * 注：.Queryable 在后台调用的就是 .Select 没区别
     */
    Company company = fsql
        .Queryable<Company>()
        .OrderByDescending(x => x.Id)
        .First();

    Company company2 = fsql
        .Select<Company>()
        .Where(x => x.CompanyName != null)
        .Skip(0)
        .Limit(1)
        .OrderByDescending(x => x.Id)
        .First();

    /* 
     * 修改
     * Sql：UPDATE [Company] SET [CompanyName] = @p_0, [CreateTime] = @p_1
            WHERE ([Id] = 6)
     */
    company.CompanyName += " Update";
    int updateAffrows = fsql.Update<Company>().SetSource(company).ExecuteAffrows();

    /*
     * 删除
     * Sql：DELETE FROM [Company] WHERE ([Id] = 1)
     */
    int deleteAffrows = fsql.Delete<Company>(company).ExecuteAffrows();
}
finally
{

}