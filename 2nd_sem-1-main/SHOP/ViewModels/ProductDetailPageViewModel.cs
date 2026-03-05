using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Npgsql;
using SHOP.Classes;

namespace SHOP.ViewModels
{
    public partial class ProductDetailPageViewModel : ViewModelBase
    {
        private readonly MainViewModel mainViewModel;

        [ObservableProperty]
        private Products? currentProduct;

        [ObservableProperty]
        private string newReviewText = string.Empty;

        private ObservableCollection<CartItem> CartItemList => ListInitializer.Instance.CartItemList;

        private Review _review;

        public IRelayCommand GoToHomeCommand { get; }
        public IRelayCommand AddReviewCommand { get; }
        public ProductDetailPageViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;

            GoToHomeCommand = new RelayCommand(() => this.mainViewModel.GoToHome());
            AddReviewCommand = new RelayCommand(AddReview);
        }

        public void UpdateProduct(Products product)
        {
            CurrentProduct = product;

            // Clear and reload the reviews for the selected product
            if (CurrentProduct != null)
            {
                var reviewHandler = new Review();
                var reviewsFromDb = reviewHandler.GetReviewsForProduct(CurrentProduct.ID);

                CurrentProduct.ReviewList.Clear();
                foreach (var review in reviewsFromDb)
                {
                    CurrentProduct.ReviewList.Add(review);
                }
            }
        }

        public void AddToCart()
        {
            for (int i = 0; i < CartItemList.Count; i++)
            {
                if (CartItemList[i].Products.EAN_Number == CurrentProduct!.EAN_Number)
                {
                    CartItemList[i].Count++;
                    mainViewModel.Total += CurrentProduct.Price;
                    mainViewModel.ProductCounter++;
                    return;
                }
            }

            if (CurrentProduct != null)
            {
                CartItemList.Add(new CartItem(CurrentProduct));
                mainViewModel.Total += CurrentProduct.Price;
            }

            mainViewModel.ProductCounter++;
        }

        private void AddReview()
        {
            try
            {
                if (CurrentProduct != null && !string.IsNullOrWhiteSpace(NewReviewText))
                {
                    if (PrivateCustomer.LoggedInCustomer != null)
                    {
                        _review = new Review(PrivateCustomer.LoggedInCustomer, NewReviewText.Trim(), CurrentProduct);
                        _review.AddReviewToDatabase(_review, CurrentProduct.ID); // først DB
                        CurrentProduct.ReviewList.Add(_review);                   // så UI
                        NewReviewText = string.Empty;
                    }
                    else
                    {
                        NewReviewText = "Please log in.";
                        Console.WriteLine("Please log in.");
                    }
                }
            }
            // 23505 is the code for unique violation
            catch (PostgresException e) when (e.SqlState == "23505") 
            {
                NewReviewText = "You have already reviewed this product.";
            }
        }

    }
}
