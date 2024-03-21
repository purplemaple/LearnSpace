namespace FreeSql.DBCore.Entities;

/// <summary>
/// 递归类型
/// </summary>
public sealed class Menu
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int Id { get; set; }

    public string? MenuName { get; set; }

    public DateTime? CreateTime { get; set; }

    public Menu Parent { get; set; }

    public int ParentId { get; set; }

    [Navigate(nameof(ParentId))]
    public List<Menu> Childs { get; set; }


    [Navigate(nameof(UserMenuMap.MenuId))]
    public List<UserMenuMap> UserMenuMapInfo { get; set; }
}
