
## `FreeSql`与`EF Core`对比

| **对比项**       | **FreeSql**                          | **EF Core**                            |
| ------------- | ------------------------------------ | -------------------------------------- |
| **性能**        | 性能较好，SQL 生成优化，查询结果干净。                | 性能较普通，某些情况下表现较慢。                       |
| **数据库支持**     | 支持更多数据库，如 MySQL、PostgreSQL、Sqlite 等。 | 主要支持常见的数据库（SqlServer、SQLite、Postgres）。 |
| **CodeFirst** | 强大的 CodeFirst 支持，灵活定制。               | 标准的 CodeFirst 支持，但灵活度稍逊。               |
| **查询方式**      | 支持复杂的 SQL 查询，易于进行联表操作。               | 使用 LINQ 查询，容易产生较复杂的查询。                 |
| **特色**        | 性能高，支持更多类型的数据库，适合需要高效数据库操作的项目。       | 丰富的 Microsoft 生态，适合与 Microsoft 产品集成。   |

## FreeSql 怎么配置成单例？

```cs
public static class FreeSqlFactory
{
    public static IFreeSql Fsql { get; } = new FreeSqlBuilder()
        .UseConnectionString(DataType.SqlServer, "your_connection_string_here")
        .UseLazyLoading(false)  // 根据需要选择开启延迟加载
        .UseMonitorCommand(cmd => Console.WriteLine(cmd.CommandText))  // 输出 SQL 到控制台
        .Build();
}
```

## FreeSql 如何进行增删改查？

