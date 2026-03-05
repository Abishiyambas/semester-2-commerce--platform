using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using SHOP.Classes;
using SHOP.ViewModels;

namespace SHOP.Views;

public partial class AccessoriesView : UserControl
{
    private AccessoriesViewModel? ViewModel => this.DataContext as AccessoriesViewModel;

    public AccessoriesView()
    {
        InitializeComponent();
        this.DataContext = new AccessoriesViewModel(new MainViewModel());
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
        {
            // Sørg for at produktet er korrekt bundet og tilgængeligt
            if ((sender as Button)?.DataContext is Products product)
            {
                ViewModel.GoToProductDetailPageCommand.Execute(product);  // Kald kommandoen med produktet
            }
        }
        else
        {
            // Fejlmeddelelse, hvis ViewModel er null
            Console.WriteLine("ViewModel is null");
        }
    }


}
