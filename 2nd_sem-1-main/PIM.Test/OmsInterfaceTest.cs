namespace PIM.Test;

using System.Threading.Tasks;


public class OMSInterfaceTests
{
    PIM_OMS_Interface.IPim Ipim;

    [SetUp]
    public void Setup ()
    {
        Ipim = new PIM_Logic.Pim ( new PIM_Data.DatabaseUtility() ); // Instaniate new Pim object declared with the interface that oms should use
    }

    [Test]
    public async Task GetProductStock ()
    {
        int testProductId = 1;      // This product has a stock of 25
        int expectedStock = 25;
        int actualStock = await Ipim.GetProductStock(testProductId);

        Assert.That ( actualStock, Is.EqualTo ( expectedStock ), $"The actual stock: {actualStock}, does not match the expected stock: {expectedStock}" );    
    }

    [Test]
    public async Task GetProductMaxStock ()
    {
        int testProductId = 1;      // This product has a max stock of 25
        int expectedMaxStock = 25;
        int actualMaxStock = await Ipim.GetProductStock(testProductId);

        Assert.That ( actualMaxStock, Is.EqualTo ( expectedMaxStock ), $"The actual stock: {actualMaxStock}, does not match the expected stock: {expectedMaxStock}" );
    }

    [Test]
    public async Task GetProductStock_InvalidProductId ()
    {
        int invalidProductId = -1;

        int actualStock = await Ipim.GetProductStock(invalidProductId);

        Assert.That ( actualStock, Is.EqualTo(0), "The stock value for an invalid product ID should be 0." );
    }

    [Test]
    public async Task GetProductMaxStock_InvalidProductId ()
    {
        int invalidProductId = -1;

        int actualMaxStock = await Ipim.GetProductStock(invalidProductId);

        Assert.That ( actualMaxStock, Is.EqualTo ( 0 ), "The stock value for an invalid product ID should be 0." );
    }
}