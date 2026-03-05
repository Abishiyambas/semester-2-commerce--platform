namespace PIM.Test;

using PIM_Logic;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Tests
{
    Pim pim;

    [SetUp]
    public void Setup ()
    {
        AppEnvironment.IsTestEnvironment = true;
        pim = new Pim ( new PIM_Data.DatabaseUtility () );
    }

    [Test]
    public async Task GetProduct ()
    {
        // husk a log på db
        Product[] products = await pim.GetAllProducts();
        Supplier[] suppliers = await pim.GetAllSuppliers();
        Category[] categories = await pim.GetAllCategories();

        foreach ( Product product in products )
        {
            if ( product.GetSku () == null )
                Assert.Fail ( "Sku should not be null" );
            if ( product.Product_id == null || product.Product_id <= 0 )
                Assert.Fail ( "product id" );
            if ( string.IsNullOrWhiteSpace ( product.Name ) )
                Assert.Fail ( "Name" );
            if ( product.Price == null || product.Price < 0 )
                Assert.Fail ( "Price" );
            if ( product.Max_stock == null || product.Max_stock < 0 )
                Assert.Fail ( "Max stock" );

            if ( product.Category_id > categories.Length && product.Category_id != null ) // categoryid can be null
                Assert.Fail ( "category id" );
            if ( product.Supplier_id == null || product.Supplier_id > suppliers.Length )
                Assert.Fail ( "supplier id" );

            if ( product.GetCategory () != null && string.IsNullOrWhiteSpace ( product.GetCategory ().Name ) )
                Assert.Fail ();
            if ( product.GetSupplier () == null || string.IsNullOrWhiteSpace ( product.GetSupplier ().Name ) )
                Assert.Fail ();
        }
        Assert.Pass ();
    }

    [Test]
    public async Task CreateProduct ()
    {
        Product[] testProducts = GetTestProducts ();
        HashSet<Product> testProductSet = new HashSet<Product>();

        foreach ( Product product in testProducts ) // Insert all the products within the set into the db
        {
            testProductSet.Add ( await pim.InsertProduct ( product ) );
        }

        Assert.That ( testProductSet.IsSubsetOf ( ( await pim.GetAllProducts () ).ToHashSet () ), "Database does not contain the new product set" ); //Check if the test set is inside of the whole database set
        foreach ( Product product in testProductSet )
        {
            await pim.DeleteProduct ( product.Product_id );
        }
    }

    [Test]
    public async Task ValidateGetProduct ()
    {
        int testProductId = 1;

        Product expectedProduct = new Product
        (
            "Laptop 15\" 16GB RAM",
            "High performance laptop with 16GB RAM and 512GB SSD",
            true,
            8999.99M,
            25,
            25,
            1,
            1
        );
        expectedProduct.SetSku ( "111,111,11" );
        Product actualProduct = await pim.GetProduct ( testProductId );

        Assert.That ( actualProduct.Equals ( expectedProduct ) );
    }

    [Test]
    public async Task ValidateGetCategory ()
    {
        int testCategoryId = 1;

        Category expectedCategory = new Category
        (
            testCategoryId,
            "Laptops"
        );
        Category actualCategory = await pim.GetCategory ( testCategoryId );

        Assert.That ( actualCategory.Equals ( expectedCategory ) );
    }

    [Test]
    public async Task ValidateGetSupplier ()
    {
        int testSupplierId = 1;

        Supplier expectedSupplier = new Supplier
        (
            testSupplierId,
            "TechSupply",
            "123456789"
        );
        Supplier actualSupplier = await pim.GetSupplier ( testSupplierId );

        Assert.That ( actualSupplier.Equals ( expectedSupplier ) );
    }

    private static Product[] GetTestProducts ()
    {
        return [
            new Product (
                "someProduct",                          //Name
                "someDescription",                      //Description
                true,                                   //Status
                1000,                                   //Price
                10,                                     //Max_stock
                1,                                      //Current_stock
                1,                                      //category id
                1                                       //GetSupplierName id
            ),
            new Product (
                "someProduct",
                null,
                true,
                1000,
                10,
                1,
                1,
                1
            ),
            new Product (
                "someProduct",
                "someDescription",
                true,
                1000,
                10,
                1,
                null,
                1
            )
        ];
    }

    private Product CreateStockTestProduct ()
    {
        return new Product
        (
            "Test Stock Product",
            "Used for stock order tests",
            true,
            100,
            50,
            0,
            1,
            1
        );
    }

    [Test]
    public async Task ValidateOrderStockMax ()
    {
        // Create test and insert product
        Product product = await pim.InsertProduct ( 
            new Product
            (
                "Test Stock Product",
                "Used for stock order tests",
                true,
                100,
                50,
                0,
                1,
                1
            ) 
        );

        // Update the stock
        product.Current_stock = product.Max_stock;
        await pim.UpdateStock ( product );

        Product updatedProduct = await pim.GetProduct( product.Product_id );

        Assert.That ( updatedProduct.Current_stock, Is.EqualTo ( product.Max_stock ), "Expected current stock to be max stock" );

        await pim.DeleteProduct ( product.Product_id );
    }

    [Test]
    public async Task ValidateOrderStockPartially ()
    {
        Product product = CreateStockTestProduct();
        await pim.InsertProduct ( product );
        product.Product_id = await pim.GetHighestProductId ();

        product.Current_stock += 25;

        await pim.UpdateStock ( product );

        Product updatedProduct = await pim.GetProduct( product.Product_id );

        Assert.That ( updatedProduct.Current_stock, Is.EqualTo ( 25 ), "Expected current stock to be max stock" );

        await pim.DeleteProduct ( product.Product_id );
    }


    private static HashSet<Employee> GetTestEmployees ()
    {
        return
        [
            new Employee("Victor Munk", "VaporeonIYKYK", true),
            new Employee("Mikkel Hebel", "HackerTrackerBingBong", true),
            new Employee("Lionel Messi", "SoyLaCabra", false)
        ];
    }

    [Test]
    public async Task CreateEmployee ()
    {
        HashSet<Employee> testEmployees = GetTestEmployees();

        foreach ( Employee employee in testEmployees )
        {
            await pim.InsertEmployee ( employee );
            employee.Employee_id = await pim.GetHighestEmployeeId ();
        }

        Assert.That ( testEmployees.IsSubsetOf ( ( await pim.GetAllEmployees () ).ToHashSet () ), "Database does not contain the new employee set" );

        foreach ( Employee employee in testEmployees )
        {
            await pim.DeleteEmployee ( employee );
        }
    }

    [Test]
    public async Task ValidateGetEmployee ()
    {
        int testEmployeeId = 1;

        Employee expectedEmployee = new Employee
        (
            "Arne",
            "password",
            true
        )
        {
            Employee_id = 1
        };


        Employee actualEmployee = await pim.GetEmployee ( testEmployeeId );

        Assert.That ( actualEmployee.Equals ( expectedEmployee ) );
    }

    //Category
    private static HashSet<Category> GetTestCategories ()
    {
        return
        [
            new Category("Football"),
            new Category("Fruits"),
            new Category("Sport")
        ];
    }

    [Test]
    //Så snart det sættes ind, tjekkes det om de er sammen med de andre cateorier
    public async Task CreateCategory ()
    {
        HashSet<Category> testCategories = GetTestCategories();

        foreach ( Category category in testCategories )
        {
            await pim.InsertCategory ( category );
            category.Category_id = (int) await pim.GetCategoryId ( category.Name );

        }

        //Kigger om mine 3 nye categorier eksistere 
        Assert.That ( testCategories.IsSubsetOf ( ( await pim.GetAllCategories () ).ToHashSet () ),
            "Database does not contain the new category set" );

        foreach ( Category category in testCategories )
        {
            await pim.DeleteCategory ( category.Category_id );
        }
    }
}