using System;

namespace PIM_Logic;

public class Discount
{
    public int Discount_id { get; set; }
    public int Discount_percentage { get; set; }
    public DateTime Start_date { get; set; }
    public DateTime End_date { get; set; }
    public int? Product_id { get; set; }
    public int? Category_id { get; set; }
    

    public Discount(Npgsql.NpgsqlDataReader reader)
    {
        Discount_id = reader.GetInt32 ( 0 );
        Discount_percentage = reader.GetInt32 ( 1 );
        Start_date = reader.GetDateTime ( 2 );
        End_date = reader.GetDateTime ( 3 );
        Product_id = reader.IsDBNull(4) ? null : reader.GetInt32 ( 4 );
        Category_id = reader.IsDBNull(5) ? null : reader.GetInt32 ( 5 );
    }

    public Discount(int percentage, DateTime startDate, DateTime endDate, int? productId, int? categoryId)
    {
        Discount_percentage = percentage;
        Start_date = startDate;
        End_date = endDate;
        Product_id = productId;
        Category_id = categoryId;
    }

    public bool IsActive()
    {
        var now = DateTime.Now;
        return now >= Start_date && now <= End_date;
    }

    public override bool Equals ( object? obj )
    {
        if ( obj is not Discount || obj is null)
            return false;


        Discount other = (Discount) obj;

        return Discount_id == other.Discount_id;
    }

    public override int GetHashCode ()
    {
        return Discount_id.GetHashCode ();
    }
}