namespace _1_MapsterQuickStart;

internal class User
{
    public int Id;
    public string? FirstName;
    public string? LastName;

    public override string ToString()
    {
        return "User { Id = " + Id + "， " + "FirstName = " + FirstName + ", " + "LastName = " + LastName + " }";
    }
}

