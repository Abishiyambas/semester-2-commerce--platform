using System;

namespace PIM_Logic;

public class PriceHistory
{
    public int Price_history_id { get; set; }
    public DateTime Changed_at { get; set; }
    public decimal New_price { get; set; }
    public decimal Old_price { get; set; }
    public int Product_id { get; set; }

    public PriceHistory ( Npgsql.NpgsqlDataReader reader )
    {
        Price_history_id = reader.GetInt32 ( 0 );
        Changed_at = reader.GetDateTime ( 1 );
        New_price = reader.GetDecimal ( 2 );
        Old_price = reader.GetDecimal ( 3 );
        Product_id = reader.GetInt32 ( 4 );
    }

    public PriceHistory(decimal newPrice, decimal oldPrice, int productId)
    {
        Changed_at = DateTime.Now;
        New_price = newPrice;
        Old_price = oldPrice;
        Product_id = productId;
    }
}