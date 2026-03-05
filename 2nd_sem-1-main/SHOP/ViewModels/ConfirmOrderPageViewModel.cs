using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SHOP.Views;
using SHOP.Classes;
using Tmds.DBus.Protocol;

namespace SHOP.ViewModels;

public partial class ConfirmOrderPageViewModel : ViewModelBase
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
    

    public OverviewViewModel OverViewModel { get; }
    
    public IRelayCommand GoToHomeCommand { get; }
    public IRelayCommand GoToOrderCommand { get; }

    public ObservableCollection<CartItem> CartItemList => ListInitializer.Instance.CartItemList;

    public ConfirmOrderPageViewModel(OverviewViewModel overviewViewModel, MainViewModel mainVm)
    {
        OverViewModel = overviewViewModel;
        Name = overviewViewModel.Name;
        Address = overviewViewModel.Address;
        PostalCode = overviewViewModel.PostalCode;
        City = overviewViewModel.City;
        Country = overviewViewModel.Country;
        Telephone = overviewViewModel.Telephone;
        Email = overviewViewModel.Email;
        TotalPrice = overviewViewModel.TotalPrice;
        VatPrice = overviewViewModel.VatPrice;
        DeliveryPrice = overviewViewModel.DeliveryPrice;
        PriceWithDelivery = overviewViewModel.PriceWithDelivery;
       
         
        GoToHomeCommand = new RelayCommand(() => mainVm.GoToHome());
        GoToOrderCommand = new RelayCommand(execute: () => mainVm.GoToOrderHistory());
    }
}