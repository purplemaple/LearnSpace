namespace FreeSql.DBCore.Entities;

public sealed class CompanyCopy
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int Id { get; set; }

    public string? CompanyName { get; set; }

    public DateTime CreateTime { get; set; }
}
