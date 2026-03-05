namespace PIM.Test;

using System.Threading.Tasks;

public class SHOPInterfaceTests
{
    PIM_SHOP_Interface.IPim Ipim;

    [SetUp]
    public void Setup ()
    {
        Ipim = (PIM_SHOP_Interface.IPim) new PIM_Logic.Pim ( new PIM_Data.DatabaseUtility () ); // Instaniate new Pim object declared with the interface that shop should use
    }

    [Test]
    public async Task IShopPimGetProductInfo ()
    {
        //Get all the product ids that exits in the db
        int[] allProductIds = await Ipim.GetAllProductIds();

        // Go trough each id and test if they throw an exception
        foreach ( int id in allProductIds )
        {
            try
            {
                Avalonia.Controls.TextBlock productNameTextBlock = new Avalonia.Controls.TextBlock() { Text = await Ipim.GetProductName ( id ) };
                Avalonia.Controls.TextBlock productDescriptionTextBlock = new Avalonia.Controls.TextBlock() { Text = await Ipim.GetProductName ( id ) };
                Avalonia.Controls.TextBlock productPriceTextBlock = new Avalonia.Controls.TextBlock() { Text = await Ipim.GetProductName ( id ) };
                Avalonia.Controls.TextBlock productStockTextBlock = new Avalonia.Controls.TextBlock() { Text = await Ipim.GetProductName ( id ) };
                Avalonia.Controls.TextBlock productStatusTextBlock = new Avalonia.Controls.TextBlock() { Text = await Ipim.GetProductName ( id ) };
                Avalonia.Controls.TextBlock productCategoryTextBlock = new Avalonia.Controls.TextBlock() { Text = await Ipim.GetProductName ( id ) };
                Avalonia.Controls.TextBlock productSupplierTextBlock = new Avalonia.Controls.TextBlock() { Text = await Ipim.GetProductName ( id ) };
            }
            catch ( NullReferenceException )
            {
                Assert.Fail ();
            }
            catch ( Exception )
            {
                Assert.Fail ();
            }
        }
        Assert.Pass ();
    }

    [Test]
    public async Task IShopPimGetProductReviews ()
    {
        int[] allProductIds = await Ipim.GetAllProductIds();

        foreach ( int id in allProductIds )
        {
            try
            {
                Tuple<int, string>[] productReviews = await Ipim.GetAllProductReviews ( id );
                foreach ( Tuple<int, string> reviewTuple in productReviews)
                {
                    Avalonia.Controls.TextBlock reviewScoreTextBlock = new Avalonia.Controls.TextBlock() {Text = reviewTuple.Item1.ToString()};
                    Avalonia.Controls.TextBlock reviewTextTextBlock = new Avalonia.Controls.TextBlock() {Text = reviewTuple.Item2};
                }
            }
            catch(NullReferenceException)
            {
                Assert.Fail ();
            }
            catch ( Exception )
            {
                Assert.Fail ();
            }
        }
        Assert.Pass ();
    }

    [Test]
    public async Task ValidateGetProductInfo ()
    {
        int testProductId = 1;

        string expectedName = "Laptop 15\" 16GB RAM";
        string expectedDescription = "High performance laptop with 16GB RAM and 512GB SSD";
        bool expectedStatus = true;
        double expectedPrice = 8999.95;     
        int expectedStock = 25;
        string expectedCategory = "Laptops";
        string expectedSupplier = "TechSupply";

        string actualName = await Ipim.GetProductName ( testProductId );
        string actualDescription = await Ipim.GetProductDescription ( testProductId );
        double actualPrice = await Ipim.GetProductPrice ( testProductId );
        int actualStock = await Ipim.GetProductStock ( testProductId );
        bool actualStatus = await Ipim.GetProductStatus ( testProductId );
        string actualCategory = await Ipim.GetProductCategory ( testProductId );
        string actualSupplier = await Ipim.GetProductSupplier ( testProductId );

        // Assert that all actual values match the expected values
        Assert.That ( actualName, Is.EqualTo ( expectedName ), $"The actual name: {actualName}, does not match the expected name: {expectedName}" );
        Assert.That ( actualDescription, Is.EqualTo ( expectedDescription ), $"The actual description: {actualDescription}, does not match the expected description: {expectedDescription}" );
        Assert.That ( actualPrice, Is.EqualTo ( expectedPrice ), $"The actual price: {actualPrice}, does not match the expected price: {expectedPrice}" );
        Assert.That ( actualStock, Is.EqualTo ( expectedStock ), $"The actual stock: {actualStock}, does not match the expected stock: {expectedStock}" );
        Assert.That ( actualStatus, Is.EqualTo ( expectedStatus ), $"The actual status: {actualStatus}, does not match the expected status: {expectedStatus}" );
        Assert.That ( actualCategory, Is.EqualTo ( expectedCategory ), $"The actual category: {actualCategory}, does not match the expected category: {expectedCategory}" );
        Assert.That ( actualSupplier, Is.EqualTo ( expectedSupplier ), $"The actual supplier: {actualSupplier}, does not match the expected supplier: {expectedSupplier}" );
    }
}