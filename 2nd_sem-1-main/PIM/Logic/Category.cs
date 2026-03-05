namespace PIM_Logic;

public class Category
{
    public int Category_id { 
        get { return id; } 
        set { id = value; } 
    }
    public string Name { 
        get { return name; } 
        set { name = value; } }
    
    private int id;
    private string name;
    private int productCount;

    public Category ( int id, string name )
    {
        Category_id = id;
        Name = name;
    }
    public Category ( string name )
    {
        Name = name;
    }

    public Category ( Npgsql.NpgsqlDataReader reader )
    {
        id = reader.GetInt32 ( 0 );
        name = reader.GetString ( 1 );
    }

    public void SetProductCount(int count)
    {
        this.productCount = count;
    }

    public int GetProductCount()
    {
        return this.productCount;
    }

    public override bool Equals ( object? obj )
    {
        if ( obj == null || GetType () != obj.GetType () )
            return false;

        Category otherProduct = (Category) obj;
        return Name == otherProduct.Name;
    }

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
}