using PIM_Logic;
using PIM_Presentation;

namespace PIM.Test;

[TestFixture]
[TestOf(typeof(StatisticsPage))]
public class StatisticsTests
{
    private StatisticsPage page;
    Product[] products;
    private Pim pim;

    [SetUp]
    public async Task Setup()
    {
        AppEnvironment.IsTestEnvironment = true;
        pim = new Pim ( new PIM_Data.DatabaseUtility () );
        page = new StatisticsPage(pim);
        products = ( await pim.GetAllProducts () ).Take ( 8 ).ToArray (); // run test on the first 8 product which are known tests products
    }

    [Test]
    public void FilterProducts_ShouldReturnMatchingProducts()
    {
        Assert.That(page.FilterProducts( products, "Laptop" ).Length, Is.EqualTo(2));
        Assert.That(page.FilterProducts( products, "Soundbar").Length, Is.EqualTo(1));
    }

    [Test]
    public void FilterProducts_ShouldBeCaseInsensitive()
    {
        Assert.That(page.FilterProducts( products, "blUEtOotH heAdPhoNES").Length, Is.EqualTo(1));
    }

    [Test]
    public void FilterProducts_ShouldReturnEmptyWhenNoMatch()
    {
        Assert.That(page.FilterProducts(products, "nonexisting product AAAAAAAARRR").Length, Is.EqualTo(0));
    }
}