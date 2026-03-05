using Avalonia.Controls;
using Avalonia.Interactivity;
using PIM_Logic;
namespace PIM_Presentation
{
    public partial class RemoveItemWindow : Window
    {
        public RemoveItemWindow(Product product)
        {
            InitializeComponent();
            NameRow.Text = product.Name;
            CategoryRow.Text = product.GetCategory ().Name;
            SupplierRow.Text = product.GetSupplier ().Name;
            PriceRow.Text = product.Price.ToString();
        }

        private void Cancel_Click(object? sender, RoutedEventArgs e)
        {
            Close(false); // return false when canceled
        }

        private void Delete_Click(object? sender, RoutedEventArgs e)
        {
            // Vi returnerer true til det vindue der åbnede dette
            Close(true); 
        }
    }
}