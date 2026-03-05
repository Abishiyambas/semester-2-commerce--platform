using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Dapper;
using Npgsql;
using System.Linq;
using SHOP;
using Avalonia.Interactivity;
using System;
using SHOP.Classes;
using SHOP.ViewModels;

namespace SHOP.Views;

public partial class LaptopView : UserControl
{
    public LaptopViewModel? ViewModel => this.DataContext as LaptopViewModel;

    public LaptopView()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var product = (sender as Button)?.DataContext as Products;
        if (product != null)
        {
            ViewModel?.GoToProductDetailPageCommand.Execute(product);
        }
    }


}