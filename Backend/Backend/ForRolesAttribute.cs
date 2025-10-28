namespace Backend;

public class ForRolesAttribute : Attribute
{
    public List<string> roles { get; set; } = new List<string>();
    public ForRolesAttribute(List<string> roles)
    {
        this.roles = roles;
    }
}