using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using Dapper;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using PIM_OMS_Interface;
using SHOP.ViewModels;

namespace SHOP.Classes;

public class Order
{
    private readonly Credentials.Credentials _credentials = new();
    private PIM_SHOP_Interface.IPim iPim;
    public int Id { get; protected set; }
    public CartViewModel CartViewModel { get; set; }
    public string OrderNumber { get; protected set; }
    public DateTime OrderDate { get; set; }
    public string ShippingAddress { get; set; }
    public string Zip { get; set; }
    
    public List<CartItem> CartItems { get; set; } = new();

    public PrivateCustomer PrivateCustomer { get; set; }
    public BusinessCustomer BusinessCustomer { get; set; }

    public List<(Products Product, int Quantity)> Products { get; set; } = new();

    public Order(PrivateCustomer privateCustomer, string shippingAddress, string zip)
    {
        OrderNumber = OrderNumberGenerator();
        OrderDate = DateTime.Now;
        ShippingAddress = shippingAddress;
        Zip = zip;
        PrivateCustomer = privateCustomer;
    }

    public Order(BusinessCustomer businessCustomer, string shippingAddress, string zip)
    {
        OrderNumber = OrderNumberGenerator();
        OrderDate = DateTime.Now;
        ShippingAddress = shippingAddress;
        Zip = zip;
        BusinessCustomer = businessCustomer;
    }

    public string OrderNumberGenerator()
    {
        return Guid.NewGuid().ToString("N")[..10]; // N then 10 random characters
    }

    public void AddProduct(Products product, int quantity = 1)
    {
        Products.Add((product, quantity));
    }

    public void AddOrderToDatabase(User user, ObservableCollection<CartItem> cartItems, String address, string zip)
    {
        if (PrivateCustomer == null && BusinessCustomer == null)
        {
            Console.WriteLine("Order must be associated with a customer.");
            return;
        }

        const string insertOrderSql = @"
        INSERT INTO orders 
            (order_number, shipping_address, zip, date, customer_id)
        VALUES 
            (@OrderNumber, @ShippingAddress, @Zip, @OrderDate, @CustomerId)
        RETURNING id";

        const string insertOrderProductSql = @"";
        string OrderNumberGenerated = OrderNumberGenerator();        
        using var connection = new NpgsqlConnection(_credentials.Database);
        connection.Open();

        connection.Execute(insertOrderSql, new
        {
            CustomerId = user.Id,
            OrderNumber = OrderNumberGenerated,
            ShippingAddress = address,
            Zip = zip,
            OrderDate = DateTime.Now,
        });
        
        const string inserOrderLineSQL =
            @"INSERT INTO order_lines(order_number, product_id,amount) VALUES(@OrderNumber, @ProductId, @Amount)";
        
        foreach (var cartitem in cartItems)
        {
            connection.Execute(inserOrderLineSQL, new
            {
                OrderNumber = OrderNumberGenerated,
                ProductId = cartitem.Products.ID,
                Amount = cartitem.Count
            });
        }
    }

   public async Task<ObservableCollection<OrderItem>> GetOrderHistory(User user)
    {
        iPim = new PIM_Logic.Pim ( new PIM_Data.DatabaseUtility () );
        var orderHistory = new ObservableCollection<OrderItem>();

        string query = @"
            SELECT p.id, order_lines.amount, orders.date
            FROM orders
            INNER JOIN order_lines ON orders.order_number = order_lines.order_number
            INNER JOIN products p ON order_lines.product_id = p.id
            INNER JOIN customers ON orders.customer_id = customers.id
            WHERE customers.id = @UserId;
        ";

        using var connection = new NpgsqlConnection(_credentials.Database);
        connection.Open();

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserId", user.Id);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int amount = reader.GetInt32(1);
            DateTime date = reader.GetDateTime(2);

            Task<string> name = iPim.GetProductName( id );
            Task<string> supplier = iPim.GetProductSupplier ( id );
            Task<string> sku = iPim.GetProductSKU ( id );
            Task<double> price = iPim.GetProductPrice ( id );
            Task<string> category = iPim.GetProductCategory ( id );
            Task<string> description = iPim.GetProductDescription ( id );
            Task<string> filePath = ListInitializer.Instance.GetProductFilepath(id);

            Task[] tasks =
            [
                name,
                supplier,
                sku,
                price,
                category,
                description,
                filePath
            ];
            await Task.WhenAll(tasks);

            var product = new Products()
            {
                ID = Id,
                Name = await name,
                Manufacturer = await supplier,
                Price = await price,
                Type = await category,
                DescriptionPath = await description,
                FilePath = await filePath
            };

            var orderItem = new OrderItem(product, amount, date);
            orderHistory.Add(orderItem);

        }
        return orderHistory;
    }

}
