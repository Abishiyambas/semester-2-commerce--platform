using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SHOP.Classes;
using CommunityToolkit.Mvvm.Input;
using SHOP.Views;
using Tmds.DBus.Protocol;

namespace SHOP.ViewModels;

public partial class PaymentCardViewModel: ViewModelBase
{
    [ObservableProperty] private string? cardNumber;
    [ObservableProperty] private string? cardDateYearExpire;
    [ObservableProperty] private string? cardDateMonthExpire;
    [ObservableProperty] private string? cardCVR;
    [ObservableProperty] private double? price;
    [ObservableProperty] private string? address;
    [ObservableProperty] private string? postalCode;
    [ObservableProperty] private string? name;
    [ObservableProperty] private string? email;
    [ObservableProperty] private string? telephoneNumber;
    [ObservableProperty] private string? city;
    [ObservableProperty] private string? country;

    
    public IRelayCommand GoToConfirmOrderPageCommand { get; }
    
    public PaymentCardViewModel(OverviewViewModel overviewViewModel, MainViewModel mainVm)
    {
        Name = overviewViewModel.Name;
        Email = overviewViewModel.Email;
        TelephoneNumber = overviewViewModel.Telephone;
        City = overviewViewModel.City;
        Country = overviewViewModel.Country;
        Price = overviewViewModel.PriceWithDelivery;
        Address = overviewViewModel.Address;
        PostalCode = overviewViewModel.PostalCode;
        GoToConfirmOrderPageCommand = new RelayCommand(() => mainVm.GoToConfirmOrder());
    }
        
}

