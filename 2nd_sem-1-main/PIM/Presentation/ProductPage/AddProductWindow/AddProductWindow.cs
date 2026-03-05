using Avalonia.Controls;
using Avalonia.Media;

using PIM_Exceptions;
namespace PIM_Presentation;

public partial class AddProductWindow : Window
{
    private string GetName()
    {
        if ( string.IsNullOrWhiteSpace ( ProductNameBox.Text ) )
            throw new InvalidTextBoxException ( "ProductNameBox does not contain a value", ProductNameBox );
        return ProductNameBox.Text;
    }

    private string? GetDescription()
    {
        return ProductDescriptionBox.Text;
    }

    private int GetMaxStock()
    {
        if ( MaxStockNumericUpDown.Value == null )
            throw new InvalidNumericUpDownException( "MaxStockNumericUpDown cannot be null", MaxStockNumericUpDown );

        if ( MaxStockNumericUpDown.Value <= 0 )
            throw new InvalidNumericUpDownException ( $"MaxStockNumericUpDown cannot be {MaxStockNumericUpDown.Value} or lower", MaxStockNumericUpDown );

        return (int) MaxStockNumericUpDown.Value;
    }

    private decimal GetPrice()
    {
        // If the text in the price textbox cannot be parse to decimal we throw an exception 
        if ( !decimal.TryParse ( ProductPriceBox.Text, out decimal price ) )
            throw new InvalidTextBoxException ( "Invalid parse of price to decimal", ProductPriceBox );
        // The price should not be 0 or lower and will throw an exception 
        if ( price <= 0 )
            throw new InvalidTextBoxException ( "Price cannot be set to 0 or lower", ProductPriceBox );

        return price;
    }

    private int GetCategoryId ()
    {
        if ( ProductCategoryComboBox.SelectedValue == null )
            throw new InvalidComboBoxException ( "ProductCategoryComboBox cannot be null", ProductCategoryComboBox );

        return categoryNameIdPairs[ProductCategoryComboBox.SelectedItem.ToString ()];
    }

    private int GetSupplierId()
    {
        if ( ProductSupplierComboBox.SelectedValue == null )
            throw new InvalidComboBoxException ( "ProductSupplierComboBox cannot be null", ProductSupplierComboBox);

        return supplierNameIdPairs[ProductSupplierComboBox.SelectedItem.ToString()];
    }

    private void Reset ()
    {
        ProductNameBox.Text = string.Empty;
        ProductPriceBox.Text = string.Empty;
        ProductDescriptionBox.Text = string.Empty;
        ProductCategoryComboBox.SelectedIndex = -1;
        ProductSupplierComboBox.SelectedIndex = -1;
        MaxStockNumericUpDown.Value = 0;
    }

    private void SetProductCategoryComboBoxItems ( PIM_Logic.Category[] categories )
    {
        foreach ( PIM_Logic.Category category in categories )
        {
            ProductCategoryComboBox.Items.Add ( category.Name );
        }
    }

    private void SetProductSupplierComboBoxItems ( PIM_Logic.Supplier[] suppliers )
    {
        foreach ( PIM_Logic.Supplier supplier in suppliers )
        {
            ProductSupplierComboBox.Items.Add ( supplier.Name );
        }
    }

    private void ResetControlBorders ()
    {
        ProductNameBox.BorderBrush = Brushes.Black;
        ProductPriceBox.BorderBrush = Brushes.Black;
        ProductDescriptionBox.BorderBrush = Brushes.Black;
        ProductCategoryComboBox.BorderBrush = Brushes.Black;
        ProductSupplierComboBox.BorderBrush = Brushes.Black;
        MaxStockNumericUpDown.BorderBrush = Brushes.Black;
    }
}