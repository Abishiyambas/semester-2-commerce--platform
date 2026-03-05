using System;
using SHOP;
using SHOP.Classes;
using NUnit.Framework;

namespace arneselectronics.Tests.Classes;

[TestFixture]
[TestOf(typeof(Products))]
public class ProductsTest
{
    private Products products;
    [SetUp]
    public void Setup()
    {
        products = new Products("123","NewProduct","Asus","2123232",69.0,"Labtop","Labtop.jpg" , "A labtop for school");
    }

    [Test]
    public void TestSetName()
    {
        products.Name = "Dell Bærbar";
        Assert.That(products.Name, Is.EqualTo("Dell Bærbar"));
    }

    [Test]
    public void TestProducts_GetInstance()
    {
        Assert.That(products, Is.Not.Null);
    }

    [Test]
    public void TestgetId()
    {
        Assert.That(products.ID, Is.EqualTo("123"));
    }

    [Test]
    public void TestgetName()
    {
        Assert.That(products.Name, Is.EqualTo("NewProduct"));
    }

    [Test]
    public void TestgetManufacturer()
    {
        Assert.That(products.Manufacturer, Is.EqualTo("Asus"));
    }
    
    [Test]
    public void TestgetEan()
    { 
        Assert.That(products.EAN_Number, Is.EqualTo("2123232"));
    }

    [Test]
    public void TestgetPrice()
    {
        Assert.That(products.Price, Is.EqualTo(69));
    }
        
    [Test]
    public void TestgetType()
    {
        Assert.That(products.Type, Is.EqualTo("Labtop"));
    }

    [Test]
    public void TestgetFilePath()
    {
        Assert.That(products.FilePath, Is.EqualTo("Labtop.jpg"));
    }

    [Test]
    public void TestgetDescription()
    {
        Assert.That(products.DescriptionPath, Is.EqualTo("A labtop for school"));
    }

    [TearDown]
    public void TearDown()
    {
        Console.WriteLine("Teardown");
    }
}
