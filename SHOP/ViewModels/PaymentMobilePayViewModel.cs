using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SHOP.Classes;
using CommunityToolkit.Mvvm.Input;
using SHOP.Views;
using Tmds.DBus.Protocol;

namespace SHOP.ViewModels;

public partial class PaymentMobilePayViewModel: ViewModelBase
{
    [ObservableProperty] private string? name;
    [ObservableProperty] private string? email;
    [ObservableProperty] private string? telephoneNumber;
    [ObservableProperty] private double? price;
    [ObservableProperty] private string? address;
    [ObservableProperty] private string? city;
    [ObservableProperty] private string? country;
    [ObservableProperty] private string? postalCode;
    
    
    public IRelayCommand GoToConfirmOrderPageCommand { get; }
    public PaymentMobilePayViewModel(OverviewViewModel overviewViewModel, MainViewModel mainVm)
    {
        Price = overviewViewModel.PriceWithDelivery;
        Name = overviewViewModel.Name;
        Email = overviewViewModel.Email;
        City = overviewViewModel.City;
        Country = overviewViewModel.Country;
        Address = overviewViewModel.Address;
        PostalCode = overviewViewModel.PostalCode;
        TelephoneNumber = overviewViewModel.Telephone;
        
        GoToConfirmOrderPageCommand = new RelayCommand(() => mainVm.GoToConfirmOrder());
    }
        
}