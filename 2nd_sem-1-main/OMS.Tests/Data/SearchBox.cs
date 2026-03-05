using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OMS.Data;
using OMS.Logic;

namespace OMS.Tests.Data;

[TestFixture]
[TestOf(typeof(Search_Box))]
public class SearchBoxTest
{
    private Order order;
    private Order order1;
    private Order order2;
    private List<Order> searchedOrders;

    [SetUp]
    public void Setup()
    {
        order = new Order(9, 1, 1, 1, 9000, "packed", DateTime.Today);
        order1 = new Order(10, 1, 1, 1, 500, "packed", DateTime.Today);
        order2 = new Order(2, 1, 1, 1, 100000, "packed", DateTime.Today);

        searchedOrders = new List<Order> { order, order1, order2 };
    }

    
    [Test]
    public void Searched_Order_Test()
    {
        searchedOrders = Search_Box.searched_orders(searchedOrders, 9);
        
        Assert.That(searchedOrders[0].GetOrderId, Is.EqualTo(9));
       
        Console.WriteLine(searchedOrders[0]);

        
    }
    [Test]
    public void Searched_Orderlist_Contains_1_Order()
    {
        searchedOrders = Search_Box.searched_orders(searchedOrders, 9);
        
        Assert.That(searchedOrders.Count, Is.EqualTo(1));
       
        
            Console.WriteLine(searchedOrders[0]);
        

        
    }
}