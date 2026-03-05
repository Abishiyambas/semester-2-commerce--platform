using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using SHOP.Classes;
using SHOP.ViewModels;

namespace SHOP.Views;

public partial class DesktopView : UserControl
{
    private DesktopViewModel? ViewModel => this.DataContext as DesktopViewModel;

    public DesktopView()
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
        else
        {
            Console.Write("Product not found");
        }
    }


}