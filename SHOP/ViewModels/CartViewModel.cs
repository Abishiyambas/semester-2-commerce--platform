    using System;
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using SHOP.Classes;

    namespace SHOP.ViewModels
    {

    public partial class CartViewModel : ViewModelBase
    {
        private readonly MainViewModel mainVm;
        [ObservableProperty] private double? totalPrice;
        [ObservableProperty] private double? vatPrice;
        [ObservableProperty] private bool userLoggedIn;
        public Order order;
        
        
        public ProductDetailPageViewModel ProductDetailPageViewModel { get; set; } = null!;

        public Products Products = null!;
        public ObservableCollection<CartItem> CartItemList => ListInitializer.Instance.CartItemList;
        public IRelayCommand GoToHomeCommand { get; }
        public IRelayCommand GoToCheckout { get; }
        public IRelayCommand GoToOrderHistory { get; }

        public CartViewModel(MainViewModel mainVm)
        {
            this.mainVm = mainVm;
            totalPrice = this.mainVm.Total;
            vatPrice = totalPrice * 0.2;
            GoToHomeCommand = new RelayCommand(() => this.mainVm.GoToHome());
            GoToCheckout = new RelayCommand((() => this.mainVm.GoToCheckout()));
            GoToOrderHistory = new RelayCommand((() => this.mainVm.GoToOrderHistory()));
            //Method found online
            //Ensures that properties get automatically updated, when changed
            this.mainVm.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(MainViewModel.Total))
                {
                    TotalPrice = this.mainVm.Total;
                    VatPrice = TotalPrice * 0.2;
                }
            };
            UserLoggedIn = MainViewModel.Instance.UserLoggedIn;
            
            
        }

        public void IncrementProductCount(CartItem cartItem)
        {
            cartItem.Count++;
            mainVm.Total += cartItem.Products.Price;
            mainVm.ProductCounter++;
        }


        public void DecrementProductCount(CartItem cartItem)
        {
            if (cartItem.Count == 1)
            {
                CartItemList.Remove(cartItem);
                mainVm.Total -= cartItem.Products.Price;
            }
            else
            {
                mainVm.Total -= cartItem.Products.Price;
            }
            mainVm.ProductCounter--;
            cartItem.Count--;
        }

        public void ClearProduct(CartItem cartItem)
        {
            mainVm.Total -= cartItem.Products.Price*cartItem.Count;
            mainVm.ProductCounter-= cartItem.Count;
            CartItemList.Remove(cartItem);
            if (CartItemList.Count == 0)
            {
                mainVm.Total = 0;
            }
        }
    } 
}