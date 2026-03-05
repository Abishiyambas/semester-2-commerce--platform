using Avalonia.Controls;
using Avalonia.Interactivity;
using PIM_Logic;

namespace PIM_Presentation
{
    public partial class DeleteDiscountWindow : Window
    {
        private readonly Pim _pim;
        private readonly Discount _discount;

        public DeleteDiscountWindow(Pim pim, Discount discount, string targetName)
        {
            InitializeComponent();
            _pim = pim;
            _discount = discount;

            // Populate the fields
            DiscountIdTextBox.Text           = discount.Discount_id.ToString();
            DiscountPctTextBox.Text          = $"{discount.Discount_percentage} %";
            StartDateTextBox.Text            = discount.Start_date.ToString("yyyy-MM-dd");
            EndDateTextBox.Text              = discount.End_date.ToString("yyyy-MM-dd");
            TargetTextBox.Text               = targetName; 
            // targetName could be product name or category name, passed in when creating the window
        }

        private void Cancel_Click(object? sender, RoutedEventArgs e)
        {
            // close without deleting
            Close(false);
        }

        private async void Delete_Click(object? sender, RoutedEventArgs e)
        {
            // call your Pim logic to delete by ID
            await _pim.DeleteDiscount(_discount);
            Close(true);
        }
    }
}