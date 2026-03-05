using Avalonia.Controls;
using Avalonia.Interactivity;
using PIM_Logic;

namespace PIM_Presentation
{
    public partial class RemoveSupplierWindow : Window
    {
        private Pim pim;
        private Supplier supplier;
        public RemoveSupplierWindow( Supplier supplier)
        {
            InitializeComponent();
            this.pim = pim;
            this.supplier = supplier;
            SupplierRow.Text = supplier.Name;
            PhoneRow.Text = this.supplier.Phone;
        }
        
        private void Cancel_Click(object? sender, RoutedEventArgs e)
        {
            Close(false);
        }
        
        private async void Delete_Click(object? sender, RoutedEventArgs e)
        {
            await pim.DeleteSupplier(supplier.Supplier_id);
            Close(true);
        }
    }
}