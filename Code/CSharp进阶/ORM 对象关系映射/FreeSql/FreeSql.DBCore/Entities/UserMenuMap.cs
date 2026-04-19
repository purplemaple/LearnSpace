namespace FreeSql.DBCore.Entities;

public class UserMenuMap
{
    [Column(IsIdentity = true, IsPrimary = true)]

    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual User UserInfo { get; set; }

    public int MenuId { get; set; }

    public virtual Menu MenuInfo { get; set; }
}
