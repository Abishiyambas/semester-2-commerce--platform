using System.Collections.ObjectModel;
using System;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SHOP.Classes;

namespace SHOP.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string test = "Test";
    
    [ObservableProperty] private string name;
    
        
    [ObservableProperty]
    private string loggedInUsername;
    
    [ObservableProperty]
    private ViewModelBase currentPage;

    private HomePageViewModel homePage;
    private readonly LaptopViewModel laptopPage;
    private readonly AccessoriesViewModel accessoriesPage;
    private readonly DesktopViewModel desktopPage;
    private readonly ResourcesViewModel resourcePage;
    private HardwareViewModel hardwarePage;
    private CartViewModel cartPage = null!;
    private readonly ProductDetailPageViewModel productDetailPage;
    private CheckoutViewModel checkoutPage;
    private OverviewViewModel overviewPage;
    private PaymentCardViewModel paymentCardPage;
    private PaymentMobilePayViewModel paymentMobilePayPage;
    private ConfirmOrderPageViewModel confirmOrderPage;
    private LogInViewModel logInPage;
    private RegisterUserViewModel registerUserPage;
    private FilteredSearchViewModel filteredSearchPage;
    private OrderHistoryViewModel orderHistoryPage;
    private static MainViewModel? _instance;
    public static MainViewModel Instance => _instance ??= new MainViewModel();
    
    // Objects to use in display in main window
    [ObservableProperty] private double total;
    [ObservableProperty] private double productCounter;
    [ObservableProperty] private bool userLoggedIn;
        
   // Lists
   [ObservableProperty]
   private ObservableCollection<Products> filteredProductsList = ListInitializer.Instance.FilteredProductsList;

    private readonly HelpViewModel helpPage = new HelpViewModel();

    public MainViewModel()
    {
        homePage = new HomePageViewModel(this);
        currentPage = homePage;
        accessoriesPage = new AccessoriesViewModel(this);
        desktopPage = new DesktopViewModel(this);
        laptopPage = new LaptopViewModel(this);
        productDetailPage = new ProductDetailPageViewModel(this);
        resourcePage = new ResourcesViewModel();
        logInPage = new LogInViewModel(this);
        PrivateCustomer.OnLoginSuccess += HandleLoginSuccess;
    }
    [RelayCommand]
    public void GoToHome(){
        homePage = new HomePageViewModel(this);
        CurrentPage = homePage;
    }

    [RelayCommand]
    private void GoToLaptop() =>  CurrentPage = laptopPage;

    [RelayCommand]
    private void GoToAccessories() => CurrentPage = accessoriesPage;

    [RelayCommand]
    private void GoToDesktop() => CurrentPage = desktopPage;
 
    [RelayCommand]
    private void GoToResource() => CurrentPage = resourcePage;

    [RelayCommand]
    private void GoToHardware()
    {
        hardwarePage = new HardwareViewModel(this);
        CurrentPage = hardwarePage;
    }

    [RelayCommand]
    private void GoToCart()
    {
        cartPage = new CartViewModel(this);
        CurrentPage = cartPage;
    }

    [RelayCommand]
    public void GoToProductDetailPage(Products product)
    {
        productDetailPage.UpdateProduct(product);
        CurrentPage = productDetailPage;
    }
    

    [RelayCommand]
    private void GoToLogIn() => CurrentPage = logInPage;
    
    [RelayCommand]
    private void GoToHelp() => CurrentPage = helpPage;
    
    [RelayCommand]
    public void GoToCheckout()
    {
        checkoutPage = new CheckoutViewModel(this);
        CurrentPage = checkoutPage;
    }

    [RelayCommand]
    public void GoToOverview()
    {
        overviewPage = new OverviewViewModel(checkoutPage, this);
        CurrentPage = overviewPage;
    } 
    [RelayCommand]
    public void GoToPaymentCard()
    {
        paymentCardPage = new PaymentCardViewModel(overviewPage, this);
        CurrentPage = paymentCardPage;
    }
    
    [RelayCommand]
    public void GoToPaymentMobilePay()
    {
        paymentMobilePayPage = new PaymentMobilePayViewModel(overviewPage, this);
        CurrentPage = paymentMobilePayPage;
    }
      
    [RelayCommand]
    public void GoToConfirmOrder()
    {
        confirmOrderPage = new ConfirmOrderPageViewModel(overviewPage,this);
        CurrentPage = confirmOrderPage;
    }
     
    [RelayCommand]
    public void GoToRegister()
    {
        registerUserPage = new RegisterUserViewModel(this);
        CurrentPage = registerUserPage;
    }

    [RelayCommand]
    public void GoToFilteredSearch()
    {
        filteredSearchPage = new FilteredSearchViewModel(this);
        CurrentPage = filteredSearchPage;
    } 
    

    [RelayCommand]
    public void GoToOrderHistory()
    {
        orderHistoryPage = new OrderHistoryViewModel(PrivateCustomer.LoggedInCustomer, this);
        CurrentPage = orderHistoryPage;
    }
    

    private void HandleLoginSuccess(PrivateCustomer customer)
    {
        Console.WriteLine("MainViewModel received login success for: " + customer.Name);
        LoggedInUsername = customer.Name;
        UserLoggedIn = true;

    }

#if DEBUG
    public static void ResetInstance()
    {
        _instance = null;
    }
#endif

}