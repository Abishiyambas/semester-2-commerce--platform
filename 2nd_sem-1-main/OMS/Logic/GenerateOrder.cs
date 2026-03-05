using System;
using Npgsql;
using OMS.Data;

namespace OMS.Logic;

public class GenerateOrder : IOrder
{
    
    (int, int) IOrder.GetOrderConfirmation()
        {
            return (0, 0);
        }
    Order IOrder.MakeOrder(int customerId, int productId, int boughtAmount, int price)
        {
            try
            {
                using (var conn = ConnectDb.GetNewConnection())
                {
                    
                    var cmd = new NpgsqlCommand("INSERT INTO orders (customer_id, product_id, price) VALUES ({customerId}, {productId[i]}, {boughtAmount[i]} {price})", conn);
                    cmd.ExecuteNonQuery();
                    
                }
     
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            } 
            return new Order(1, 0, 0,1,7000, "Delivered", DateTime.Now);
        }
    
}