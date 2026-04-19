namespace FreeSql.DBCore.Entities;

public class Company
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int Id { get; set; }

    [Column(IsNullable = true)]
    public string? CompanyName { get; set; }

    [Column(IsNullable = true)]
    public DateTime? CreateTime { get; set; }


    [Navigate(nameof(User.CompanyId))]
    public virtual List<User> UserList { get; set; }
    //在 User 查找 CompanyId 属性，与 本实体.主键 关联
}

public sealed class CompanyDto
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int Id { get; set; }

    public string? CompanyName { get; set; }

    public DateTime CreateTime { get; set; }
}
