using NUnit.Framework;
using PIM_Logic;
using System.Collections.Generic;
using System.Linq;
using PIM_Presentation;

namespace PIM.Test;

[TestFixture]
[TestOf(typeof(ProductsPage))]
public class ProductSortingTests
{
    private ProductsPage productsPage;
    private Product[] products;

    [SetUp]
    public async Task Setup()
    {
        AppEnvironment.IsTestEnvironment = true;
        Pim pim = new Pim(new PIM_Data.DatabaseUtility());
        productsPage = new ProductsPage(pim); // Pass null for dependencies since we're testing sorting logic

        products = ( await pim.GetAllProducts () ).Take ( 8 ).ToArray (); // run test on the first 8 product which are known tests products
    }
    
    [Test]
    public void ApplySorting_ShouldNotSortForInvalidIndex()
    {
        var sortedProducts = productsPage.ApplySorting(products, -1).ToList();
        Assert.That(sortedProducts, Is.EqualTo(products));
    }
    
    [Test]
    public void ApplySorting_ShouldSortByNameAscending()
    {
        var sortedProducts = productsPage.ApplySorting(products, 0).ToList();
        Assert.That(sortedProducts[1].Name, Is.EqualTo("Bluetooth Headphones"));
        Assert.That(sortedProducts[4].Name, Is.EqualTo("Smartwatch Series 5"));
        Assert.That(sortedProducts[5].Name, Is.EqualTo("Soundbar 2.1"));
    }

    [Test]
    public void ApplySorting_ShouldSortByNameDescending()
    {
        var sortedProducts = productsPage.ApplySorting(products, 1).ToList();
        Assert.That(sortedProducts[0].Name, Is.EqualTo("Wireless Mouse"));
        Assert.That(sortedProducts[1].Name, Is.EqualTo("Wireless Charging Pad"));
        Assert.That(sortedProducts[2].Name, Is.EqualTo("Soundbar 2.1"));
        Assert.That(sortedProducts[3].Name, Is.EqualTo("Smartwatch Series 5"));
        Assert.That(sortedProducts[6].Name, Is.EqualTo("Bluetooth Headphones"));
    }

    [Test]
    public void ApplySorting_ShouldSortByRatingDescending()
    {
        var sortedProducts = productsPage.ApplySorting(products, 2).ToList();
        Assert.That(sortedProducts[1].Name, Is.EqualTo("Smartwatch Series 5"));
        Assert.That(sortedProducts[4].Name, Is.EqualTo("Bluetooth Headphones"));
        Assert.That(sortedProducts[5].Name, Is.EqualTo("Soundbar 2.1"));
        Assert.That(sortedProducts[6].Name, Is.EqualTo("Wireless Mouse"));
    }
    
    [Test]
    public void ApplySorting_ShouldSortByRatingAscending()
    {
        var sortedProducts = productsPage.ApplySorting(products, 3).ToList();
        Assert.That(sortedProducts[0].Name, Is.EqualTo("Wireless Mouse"));
        Assert.That(sortedProducts[1].Name, Is.EqualTo("Wireless Charging Pad"));
        Assert.That(sortedProducts[3].Name, Is.EqualTo("Bluetooth Headphones"));
        Assert.That(sortedProducts[6].Name, Is.EqualTo("Smartwatch Series 5"));
    }

    [Test]
    public void ApplySorting_ShouldSortByPriceDescending()
    {
        var sortedProducts = productsPage.ApplySorting(products, 4).ToList();
        Assert.That(sortedProducts[3].Name, Is.EqualTo("Soundbar 2.1"));
        Assert.That(sortedProducts[4].Name, Is.EqualTo("Smartwatch Series 5"));
        Assert.That(sortedProducts[5].Name, Is.EqualTo("Bluetooth Headphones"));
        Assert.That(sortedProducts[6].Name, Is.EqualTo("Wireless Charging Pad"));
    }

    [Test]
    public void ApplySorting_ShouldSortByPriceAscending()
    {
        var sortedProducts = productsPage.ApplySorting(products, 5).ToList();
        Assert.That(sortedProducts[0].Name, Is.EqualTo("Wireless Mouse"));
        Assert.That(sortedProducts[1].Name, Is.EqualTo("Wireless Charging Pad"));
        Assert.That(sortedProducts[2].Name, Is.EqualTo("Bluetooth Headphones"));
        Assert.That(sortedProducts[3].Name, Is.EqualTo("Smartwatch Series 5"));
    }
}