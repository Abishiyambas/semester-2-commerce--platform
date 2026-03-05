using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

using PIM_Logic;
using PIM_Exceptions;
namespace PIM_Presentation;

public partial class AddProductWindow : Window
{
    //ProductNameBox
    //ProductPriceBox
    //ProductDescriptionBox
    //ProductCategoryComboBox
    //ProductSupplierComboBox
    //MaxStockNumericUpDown
    //DoneButton
    //CancelButton

    private readonly Pim pim;
    private Dictionary<string, int> supplierNameIdPairs;
    private Dictionary<string, int> categoryNameIdPairs;
    private readonly Func<Task> refreshAction;

    public AddProductWindow (Pim pim, Func<Task> refreshAction )
    {
        InitializeComponent();
        this.pim = pim;
        SetupComboBoxs ();
        Width = 800;
        Height = 400;
        this.refreshAction = refreshAction;
    }

    private async Task SetupComboBoxs ()
    {
        Supplier[] suppliers = await pim.GetAllSuppliers ();
        supplierNameIdPairs = suppliers.ToDictionary ( supplier => supplier.Name, supplier => supplier.Supplier_id );

        Category[] categories = await pim.GetAllCategories ();
        categoryNameIdPairs = categories.ToDictionary ( category => category.Name, category => category.Category_id );

        SetProductCategoryComboBoxItems ( categories );
        SetProductSupplierComboBoxItems ( suppliers );
    }

    public async void DoneButtonHandler(Object? sender, RoutedEventArgs args)
    {
        ResetControlBorders ();
        try {
            
            var confirmWindow = new ConfirmWindow();
            bool confirmed = await confirmWindow.ShowDialog<bool>(this);
            if (!confirmed)
                return;
            
            Product product = new Product
            (
                GetName(),
                GetDescription(),
                true,
                GetPrice(),
                GetMaxStock(),
                0,
                GetCategoryId(),
                GetSupplierId()
            );
            await pim.InsertProduct ( product );
            refreshAction.Invoke ();
            Reset ();
        }
        catch ( InvalidTextBoxException e )
        {
            e.Textbox.Text = string.Empty;
            e.Textbox.BorderBrush = Brushes.Red;
        }
        catch ( InvalidComboBoxException e )
        {
            e.ComboBox.BorderBrush = Brushes.Red;
        }
        catch ( InvalidNumericUpDownException e )
        {
            e.NumericUpDown.BorderBrush = Brushes.Red;
        }
        
    }

    public void CancelButtonHandler(Object? sender, RoutedEventArgs args)
    {
        refreshAction.Invoke ();
        Close ();
    }
}