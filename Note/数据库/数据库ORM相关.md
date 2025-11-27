
##  ORM基本概述？

1. ORM 是对象与关系数据库之间的映射工具
2. ORM 解决的问题：
	1. 使用 C# 对象代替 SQL 操作数据库
	2. ORM 通常支持多个数据库，无需修改代码，只需更改数据库配置文件
	3. 避免手写 SQL，更不容易出错，提高开发效率
3. ORM 缺点：
	1. 自动生成的 SQL 不一定最优（性能风险）
	2. 对复杂查询不如手写 SQL 精准
	3. 大量使用 ORM 容易产生 N + 1 查询问题

**N + 1 查询问题：**

**定义：**
 - N+1 查询问题是指在查询时，主查询执行一次，之后每一个主查询结果的项又会发起一次查询，导致查询次数爆炸性增长。

例如：

- 你查询了 1000 个用户，然后对于每个用户，又执行一次查询去加载其相关的订单信息。这样就会发起 **1 + 1000** 次查询，导致数据库性能问题。

例子：
```cs
var users = fsql.Select<User>().ToList();  // 1 次查询用户
foreach (var user in users)
{
    var orders = fsql.Select<Order>().Where(o => o.UserId == user.Id).ToList();  // 每次查询会发起 N 次查询
}
```

解决方案：使用 **`Include`** 或 **`Join`** 语句，将所有需要的数据一次性查询出来，避免多次查询。
```cs
var usersWithOrders = fsql.Select<User>()
                           .Include(u => u.Orders)  // 一次性加载用户和订单
                           .ToList();
```
或者使用`Join`语句进行联合查询
## ORM解决了什么问题？