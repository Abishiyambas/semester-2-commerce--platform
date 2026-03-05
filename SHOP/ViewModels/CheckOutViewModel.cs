using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SHOP.Classes;

namespace SHOP.ViewModels;
    public partial class CheckoutViewModel : ViewModelBase
    {
        [ObservableProperty] private double? totalPrice;
        [ObservableProperty] private double? vatPrice;
        [ObservableProperty] private double? deliveryPrice;
        [ObservableProperty] private double? priceWithDelivery;
        [ObservableProperty] private string? name;
        [ObservableProperty] private string? address;
        [ObservableProperty] private string? postalCode;
        [ObservableProperty] private string? city;
        [ObservableProperty] private string? country;
        [ObservableProperty] private string? telephone;
        [ObservableProperty] private string? email;
        
        public ObservableCollection<CartItem> CartItemList => ListInitializer.Instance.CartItemList;
        private readonly MainViewModel mainVm;
        public IRelayCommand GoToOverviewCommand { get; }
        public CheckoutViewModel(MainViewModel mainVm)
        {
            TotalPrice = mainVm.Total;
            PriceWithDelivery = TotalPrice;
            vatPrice = PriceWithDelivery * 0.2;
            this.mainVm = mainVm; //tror denne er overflødig?
            
            GoToOverviewCommand = new RelayCommand(() => mainVm.GoToOverview());
        }

        public void DeliveryPriceChange(double newPrice)
        {
            DeliveryPrice = newPrice;
            PriceWithDelivery = DeliveryPrice + TotalPrice;
            VatPrice = PriceWithDelivery * 0.2;
        }
    }

