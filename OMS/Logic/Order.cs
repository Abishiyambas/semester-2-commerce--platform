using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using OMS.Data;

namespace OMS.Logic;

public class Order : IComparable<Order>
{
    private int _orderId;
    private int _customerId;
    private int _productId;
    private int _amount;
    private string _currentStatus;
    private int _price;
    private DateTime _orderDate;

    public int GetOrderId { get => _orderId; set => _orderId = value;}
    public int GetCustomerId { get => _customerId; set => _customerId = value;}
    public int GetProductId { get => _productId; set => _productId = value;}
    public int GetAmount { get => _amount; set => _amount = value;}
    public string GetCurrentStatus { get => _currentStatus; set => _currentStatus = value; }
    public int GetPrice { get => _price; set => _price = value;}
    public DateTime GetOrderDate { get => _orderDate; set => _orderDate = value; }

    public Order(int orderId, int customerId, int productId, int amount, int price, string currentStatus, DateTime orderDate)
    {
        _orderId = orderId;
        _customerId = customerId;
        _productId = productId;
        _amount = amount;
        _currentStatus = currentStatus;
        _price = price;
        _orderDate = orderDate;
    }
    
    public override string ToString()
    {
        return $"OrderId:{_orderId}, CustomerId:{_customerId}, Productid:{_productId}, Amount:{_amount}, Price:{_price}, CurrentStatus:{_currentStatus}, OrderDate:{_orderDate}";
    }

    public int CompareTo(Order other)
    {
        return _orderId.CompareTo(other._orderId);
    }
    
    public static List<Order> GetOrderList()
    {

        List<Order> orders = new();
        try
        {

            // Connecting to the database using will close the connection after code has run.
            using (var conn = ConnectDb.GetNewConnection())
            {
                // Joins the different values we want to visualize in the GUI.
                var cmd = new NpgsqlCommand("SELECT orders.order_id, orders.customer_id, orders.product_id, order_lines.amount, order_tracking.current_status, orders.price, orders.date FROM orders\n  INNER JOIN order_lines ON orders.order_id = order_lines.order_id\n  INNER JOIN order_tracking ON orders.order_id = order_tracking.order_id;", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    int orderId = (int)reader["order_id"];
                    int customerId = (int)reader["customer_Id"];
                    int productId = (int)reader["product_Id"];
                    int amount = (int)reader["amount"];
                    int price = (int)reader["price"];
                    string currentStatus = (string)reader["current_status"];
                    DateTime date = (DateTime)reader["date"];

                    Order newOrder = new(orderId, customerId, productId, amount, price, currentStatus, date);

                    // Adds the newOrder to list of orders that will get displayed.
                    
                    orders.Add(newOrder);
                    
                    
                }

            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
        }

        orders.Sort();
        orders.Reverse();
        return orders;
        
    }
}