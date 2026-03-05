using System.Data;

namespace PIM_Logic;

public class Employee
{
    public int Employee_id { get { return id; } set { id = value; } }
    public string Name { get { return name; } set{ name = value; } }
    public string Password { get { return password; } set { password = value; } }
    public bool Authorized { get { return authorized; } set { authorized = value; } }

    private int     id;
    private string  name;
    private string  password;
    private bool authorized;
    
    public Employee ( string name, string password, bool authorized )
    { 
        this.name = name;
        this.password = password;
        this.authorized = authorized;
    }

    public Employee ( Npgsql.NpgsqlDataReader reader )
    {
        this.id = reader.GetInt32(0);
        this.name = reader.GetString (1);
        this.password = reader.GetString (2);
        this.authorized = reader.GetBoolean (3);
    }
    
    public override bool Equals(object obj)
    {
        if (obj is not Employee other)
            return false;

        return this.id == other.id;
    }

    public override int GetHashCode()
    {
        return this.id.GetHashCode();
    }
}