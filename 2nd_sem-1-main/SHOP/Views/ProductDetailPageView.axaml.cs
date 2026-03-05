using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SHOP.Classes;
using SHOP.ViewModels;

namespace SHOP.Views
{

    public partial class ProductDetailPageView : UserControl
    {
        public ObservableCollection<Review> ReviewList => ListInitializer.Instance.ReviewList; 
        private ProductDetailPageViewModel? ViewModel => this.DataContext as ProductDetailPageViewModel;

        public ProductDetailPageView()
        {
            InitializeComponent();
        }
        
        private void AddToCart(object? sender, RoutedEventArgs e)
        {
            if (ViewModel != null) ViewModel.AddToCart();
        }
        
        private void ClearOnFocus(object? sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.Clear();
            }
        }
        
    }
    
    
    
}