using System;

namespace PIM_Logic;

public class Supplier
{
    public int Supplier_id
    {
        get => id;
        set => id = value;
    }

    public string Name
    {
        get => name;
        set => name = value;
    }

    public string Phone
    {
        get => phone;
        set => phone = value;
    }

    private int id;
    private string name;
    private string phone;

    public Supplier ( string name, string phone )
    {
        this.name = name;
        this.phone = phone;
    }

    public Supplier ( int id, string name, string phone )
    {
        this.id = id;
        this.name = name;
        this.phone = phone;
    }

    public Supplier ( Npgsql.NpgsqlDataReader reader )
    {
        id = reader.GetInt32 ( 0 );
        name = reader.GetString ( 1 );
        phone = reader.GetString ( 2 );
    }

    public override bool Equals ( object? obj )
    {
        if ( obj == null || GetType () != obj.GetType () )
            return false;

        Supplier otherSupplier = (Supplier)obj;
        return Supplier_id == otherSupplier.Supplier_id && Name == otherSupplier.Name && Phone == otherSupplier.Phone;
    }

    public override int GetHashCode ()
    {
        return HashCode.Combine ( Supplier_id, Name, Phone );
    }
}
