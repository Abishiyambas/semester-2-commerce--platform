using Avalonia.Controls;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using Dapper;
using Npgsql;
using Avalonia.Interactivity;
using SHOP.Classes;
using SHOP.ViewModels;


namespace SHOP;

public partial class MainView : Window
{
    public ObservableCollection<Products> AllProducts => ListInitializer.Instance.ProductsList;
    private ObservableCollection<Products> FilteredProducts = ListInitializer.Instance.FilteredProductsList;
    public MainView()
    {
        InitializeComponent();
    }
    
    // method for product search 
    public void searchProducts()
    {
        if (SearchBox == null || string.IsNullOrWhiteSpace(SearchBox.Text))
        {
            // sets the list in MainViewModel to the all products list if search field is empty
            MainViewModel.Instance.FilteredProductsList = new ObservableCollection<Products>(AllProducts);
        }
        else
        {
            /* Converts input from user to lowercase and checks what products contains the users input and adds them
            to the FilteredProductsList in MainViewModel.  
             */
            var searchText = SearchBox.Text.ToLower();
            var filtered = AllProducts.Where(p => p.Name.ToLower().Contains(searchText));
            MainViewModel.Instance.FilteredProductsList  = new ObservableCollection<Products>(filtered);
        }

      
    }

    private void searchProducts(object? sender, RoutedEventArgs e)
    {
        searchProducts();
        foreach (Products product in FilteredProducts)
        Console.WriteLine(product.Name);
    }
}
