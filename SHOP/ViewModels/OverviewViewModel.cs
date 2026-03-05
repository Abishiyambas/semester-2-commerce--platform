using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SHOP.Views;
using SHOP.Classes;
using Tmds.DBus.Protocol;

namespace SHOP.ViewModels;

public partial class OverviewViewModel : ViewModelBase
{
    [ObservableProperty] private string? name;
    [ObservableProperty] private string? address;
    [ObservableProperty] private string? postalCode;
    [ObservableProperty] private string? city;
    [ObservableProperty] private string? country;
    [ObservableProperty] private string? telephone;
    [ObservableProperty] private string? email;
    [ObservableProperty] private double? totalPrice;
    [ObservableProperty] private double? vatPrice;
    [ObservableProperty] private double? deliveryPrice;
    [ObservableProperty] private double? priceWithDelivery;
    
    
    public CheckoutViewModel CheckoutViewModel { get; }
    public ObservableCollection<CartItem> CartItemList => ListInitializer.Instance.CartItemList;
    
    public IRelayCommand GoToPaymentCardCommand { get; }
    public IRelayCommand GoToPaymentMobilPayCommand { get; }
    public OverviewViewModel(CheckoutViewModel checkoutViewModel, MainViewModel mainVm)
    {
        CheckoutViewModel = checkoutViewModel;
        Name = checkoutViewModel.Name;
        Address = checkoutViewModel.Address;
        PostalCode = checkoutViewModel.PostalCode;
        City = checkoutViewModel.City;
        Country = checkoutViewModel.Country;
        Telephone = checkoutViewModel.Telephone;
        Email = checkoutViewModel.Email;
        TotalPrice = checkoutViewModel.TotalPrice;
        VatPrice = checkoutViewModel.VatPrice;
        DeliveryPrice = checkoutViewModel.DeliveryPrice;
        PriceWithDelivery = checkoutViewModel.PriceWithDelivery;
        
        
        GoToPaymentCardCommand = new RelayCommand(() => mainVm.GoToPaymentCard());
        GoToPaymentMobilPayCommand = new RelayCommand(() => mainVm.GoToPaymentMobilePay());
    }
    
    
}