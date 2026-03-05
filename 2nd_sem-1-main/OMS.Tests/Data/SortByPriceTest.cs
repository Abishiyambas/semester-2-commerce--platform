using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OMS.Data;
using OMS.Logic;

namespace OMS.Tests.Data;

[TestFixture]
[TestOf(typeof(Sort_By_Price))]
public class SortByPriceTest
{
    private Order order;
    private Order order1;
    private Order order2;
    private List<Order> OrderListTest;

    [SetUp]
    public void setup()
    {
        order = new Order(1, 1, 1, 1, 9000, "packed", DateTime.Today);
        order1 = new Order(1, 1, 1, 1, 500, "packed", DateTime.Today);
        order2 = new Order(1, 1, 1, 1, 100000, "packed", DateTime.Today);

        OrderListTest = new List<Order> { order, order1, order2 };
    }

    
    [Test]
    public void SortByPrice_Low_To_High_TEST()
    {
        OrderListTest = Sort_By_Price.SortByPrice_Low_To_High(OrderListTest);
        
        Assert.That(OrderListTest[0].GetPrice, Is.EqualTo(500));
       
        foreach (Order ORDER in OrderListTest)
        {
            Console.WriteLine(ORDER);
        }

        
    }
    [Test]
    public void SortByPrice_High_To_Low()
    {
        OrderListTest = Sort_By_Price.SortByPrice_High_To_Low(OrderListTest);
        
        Assert.That(OrderListTest[0].GetPrice, Is.EqualTo(100000));

        foreach (Order ORDER in OrderListTest)
        {
            Console.WriteLine( ORDER);
        }

        
    }
}