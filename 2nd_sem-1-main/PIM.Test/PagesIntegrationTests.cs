using PIM_Logic;
using PIM_Presentation;
using SkiaSharp;
namespace PIM.Test;

public class PagesIntegrationTests
{
    Pim pim;

    [SetUp]
    public void Setup ()
    {
        AppEnvironment.IsTestEnvironment = true;
        pim = new Pim ( new PIM_Data.DatabaseUtility () );
    }

    [Test]
    public async Task ProductPageDisplayAllProducts()
    {
        ProductsPage productsPage = new ProductsPage(pim);
        await productsPage.LoadProducts ();
        HashSet<Product> expectedProductSet = (await pim.GetAllProducts()).ToHashSet();
        Assert.That(productsPage.ProductsDisplayed.ToHashSet(), Is.EqualTo( expectedProductSet ) );
    }

    [Test]
    public async Task CategoryPageDisplayAllCategories ()
    {
        CategoriesPage categoriesPage = new CategoriesPage(pim);
        await categoriesPage.LoadCategories ();
        HashSet<Category> expectedCategorySet = (await pim.GetAllCategories()).ToHashSet();
        Assert.That ( categoriesPage.CategoriesDisplayed.ToHashSet (), Is.EqualTo ( expectedCategorySet ) );
    }

    [Test]
    public async Task SupplierPageDisplayAllSuppliers ()
    {
        SuppliersPage suppliersPage = new SuppliersPage(pim);
        await suppliersPage.LoadSuppliers ();
        HashSet<Supplier> expectedsupplierSet = (await pim.GetAllSuppliers()).ToHashSet();
        Assert.That ( suppliersPage.suppliersDisplayed.ToHashSet (), Is.EqualTo ( expectedsupplierSet ) );
    }
}
