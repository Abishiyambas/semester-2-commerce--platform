using System;
using System.ComponentModel.DataAnnotations;
using SHOP;
using SHOP.Classes;
using NUnit.Framework;

namespace SHOP_Test.Classes;

[TestFixture]
[TestOf(typeof(Order))]
public class OrderTest
{
    private PrivateCustomer _privateCustomer;
    private Order _order;
    private Products _products;

    [SetUp]
    public void Setup()
    {
        _privateCustomer = new PrivateCustomer(1, "Name", "email@email.com");
        _order = new Order(_privateCustomer, "Address", "1234"); 
        _products = new Products();
    }
    
    [Test]
    public void TestOrderExists()
    {
        Assert.That(_order, Is.Not.Null);
    }

    [Test]
    public void TestOrderInformation()
    {
        Assert.That(_order.PrivateCustomer, Is.EqualTo(_privateCustomer));
        Assert.That(_order.Zip, Is.EqualTo("1234"));
        Assert.That(_order.ShippingAddress, Is.EqualTo("Address"));
    }
    
    [Test]
    public void TestOrderNumberLength()
    {
        var result = _order.OrderNumberGenerator();
        Assert.That(result.Length, Is.EqualTo(10));
    }
    
    [Test]
    public void TestOrderNumberUniqueness()
    {
        var result1 = _order.OrderNumberGenerator();
        var result2 = _order.OrderNumberGenerator();
        Assert.That(result1, Is.Not.EqualTo(result2));
    }

    [Test]
    public void TestAddProducts()
    {
        _order.AddProduct(_products, 1);
        Assert.That(_order.Products.Count, Is.EqualTo(1));
        _order.Products.Clear();
        Assert.That(_order.Products, Is.Empty);
        _order.AddProduct(_products, 5);
        Assert.That(_order.Products.Count, Is.EqualTo(5));
    }
    
    
    [TearDown]
    public void TearDown()
    {
        Console.WriteLine(_order);
    }
}