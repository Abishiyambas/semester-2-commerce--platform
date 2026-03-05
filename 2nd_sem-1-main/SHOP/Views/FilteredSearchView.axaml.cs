using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SHOP.Classes;
using SHOP.ViewModels;

namespace SHOP.Views;

public partial class FilteredSearchView : UserControl
{
    public FilteredSearchViewModel? ViewModel => this.DataContext as FilteredSearchViewModel;
    public FilteredSearchView()
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