namespace FreeSql.DBCore.Entities;

public class User
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int Id { get; set; }



    public string? UserName { get; set; }

    public DateTime? CreateTime { get; set; }

    public int Age { get; set; }

    public int UserDetailId { get; set; }

    public UserDetail? UserDetail { get; set; }

    public int? CompanyId { get; set; }

    //导航属性，ManyToOne/OneToOne
    [Navigate(nameof(CompanyId))]
    public virtual Company? CompanyInfo { get; set; }
    //在 本实体 查找 CompanyId 属性，与 Company.主键 关联


    [Navigate(nameof(UserMenuMap.UserId))]
    public virtual List<UserMenuMap> UserMenuMapInfo { get; set; }

    /// <summary>
    /// 删除
    /// </summary>
    public bool IsDeleted { get; set; }
}
