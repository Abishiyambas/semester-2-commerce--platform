using System.Collections.ObjectModel;
using SHOP;
using SHOP.Classes;
using SHOP.Credentials;
using Dapper;
using Npgsql;
using SHOP.Credentials;
using SHOP.ViewModels;

namespace SHOP_IntegrationTests;

public class OrderDatabaseTests
{
    private readonly Credentials _credentials = new();
    private Order _order;

    //Has to be async as the methods we are testing are IE LoadOrderHistory
    [Fact]
    public async void AddOrderToDatabase_InsertsOrder()
    {
        var testUser = new PrivateCustomer();
        testUser = testUser.Register("TestName", "tester@email.com", "password", "Address", "1234", "Testcity", "Denmark", "12341234");
        var loggedInUser = PrivateCustomer.LogIn("tester@email.com", "password");

        int customerId;
        //Ensure order history is empty be deleting existing purchases
        using (var conn = new NpgsqlConnection(_credentials.Database))
        {
            conn.Open();
            customerId = conn.QuerySingle<int>("SELECT id FROM customers WHERE email = @Email", new { Email = "tester@email.com" });

            const string deleteOrderLinesSql = @"
                DELETE FROM order_lines 
                WHERE order_number IN (SELECT order_number FROM orders WHERE customer_id = @customerId)";
            const string deleteOrdersSql = @"
                DELETE FROM orders 
                WHERE customer_id = @customerId";

            conn.Execute(deleteOrderLinesSql, new { customerId });
            conn.Execute(deleteOrdersSql, new { customerId });
        }

        _order = new Order(loggedInUser, "Address", "1234");
        //Adding 2 products to order
        var testCart = new ObservableCollection<CartItem>
        {
            new CartItem(new Products { ID = 1 }, 2),
            new CartItem(new Products { ID = 2 }, 3)
        };
        _order.AddOrderToDatabase(loggedInUser, testCart, "Address", "1234");
        OrderHistoryViewModel orderHistoryViewModel = new OrderHistoryViewModel(loggedInUser, MainViewModel.Instance);
        await orderHistoryViewModel.LoadOrderHistory(loggedInUser);
        Assert.Equal(2, orderHistoryViewModel.OrderHistoryList.Count);
    }
}
