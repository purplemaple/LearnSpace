namespace FreeSql.DBCore.Entities;

public sealed class UserDetail
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int Id { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }

    public int UserId { get; set; }

    public User? UserInfo { get; set; }
}
