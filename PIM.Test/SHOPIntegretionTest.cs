using PIM_Logic;
using SHOP;
using SHOP.ViewModels;

namespace PIM.Test;

public class PIM_SHOP_Integration
{
    Product[] products;
    ListInitializer listInitializer;
    [SetUp]
    public async Task Setup ()
    {
        AppEnvironment.IsTestEnvironment = true;
        Pim pim = new Pim(new PIM_Data.DatabaseUtility());
        products = await pim.GetAllProducts ();
        listInitializer = ListInitializer.Instance;
        Thread.Sleep ( 500 ); // Ensure that the products are loaded from db before starting test
    }

    [Test]
    public void HomePageProductMatch ()
    {
        HomePageViewModel homePageViewModel = new HomePageViewModel(MainViewModel.Instance);
        Assert.That ( homePageViewModel.ProductsList.Count, Is.EqualTo ( products.Length ) );
    }

    [Test]
    public void DisplayAllAccessories ()
    {
        AccessoriesViewModel accessoriesViewModel = new AccessoriesViewModel(MainViewModel.Instance);
        Assert.That ( accessoriesViewModel.AccessoriesList.Count, Is.EqualTo ( products.Count(product => product.GetCategory().Name.ToLower() == "accessories" ) ) );
    }

    [Test]
    public void DisplayAllHardware ()
    {
        HardwareViewModel hardwareViewModel = new HardwareViewModel(MainViewModel.Instance);
        Assert.That ( hardwareViewModel.HardwareList.Count, Is.EqualTo ( products.Count ( product => product.GetCategory ().Name.ToLower () == "hardware" ) ) );
    }

    [Test]
    public void DisplayAllDesktops ()
    {
        DesktopViewModel desktopViewModel = new DesktopViewModel(MainViewModel.Instance);
        Assert.That ( desktopViewModel.DesktopList.Count, Is.EqualTo ( products.Count ( product => product.GetCategory ().Name.ToLower () == "hardware" ) ) );
    }

    [Test]
    public void DisplayAllLaptops ()
    {
        LaptopViewModel laptopViewModel = new LaptopViewModel(MainViewModel.Instance);
        Assert.That ( laptopViewModel.LaptopList.Count, Is.EqualTo ( products.Count ( product => product.GetCategory ().Name.ToLower () == "hardware" ) ) );
    }
}