namespace _1_MapsterQuickStart;

internal class UserResponse : IValidatable
{
    public int Id;
    //public string? FirstName;
    //public string? LastName;

    public string? FullName;

    public string? TraceId;

    /*public override string ToString()
    {
        return "UserResponse { Id = " + Id + "， " + "FirstName = " + FirstName + ", " + "LastName = " + LastName + " }";
    }*/

    /*public override string ToString()
    {
        return "UserResponse { Id = " + Id + "， " + "FullName = " + FullName + " }";
    }*/

    public override string ToString()
    {
        return "UserResponse { Id = " + Id + "， " + "FullName = " + FullName + ", " + "TraceId = " + TraceId + " }";
    }

}

