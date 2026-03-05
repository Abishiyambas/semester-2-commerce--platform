    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    using Dapper;
    using Npgsql;
    using System.Linq;
    using SHOP;
    using Avalonia.Interactivity;
    using System.Collections.Generic;

    using System;
    using SHOP.Classes;
    using SHOP.ViewModels;

    namespace SHOP.Views;

    public partial class CartView : UserControl
    {
        public CartViewModel? ViewModel => this.DataContext as CartViewModel;
        public CartView()
        {
            InitializeComponent();
        }

        private void AddProduct(object? sender, RoutedEventArgs e)
        {
            var cartItem = (sender as Button)?.DataContext as CartItem;
            if (cartItem != null)
            {
                ViewModel?.IncrementProductCount(cartItem);
            }
            else
            {
                Console.Write("Product not found");
            }
        }
        

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            var cartItem = (sender as Button)?.DataContext as CartItem;
            if (cartItem != null)
            {
                ViewModel?.DecrementProductCount(cartItem);
            }
            else
            {
                Console.Write("Product not found");
            }
            
        }

        private void ClearProduct(object? sender, RoutedEventArgs e)
        {
            var cartItem = (sender as Button)?.DataContext as CartItem;
            if (cartItem != null)
            {
                ViewModel?.ClearProduct(cartItem);
            }
            else
            {
                Console.Write("Product not found");
            }

        }
    }