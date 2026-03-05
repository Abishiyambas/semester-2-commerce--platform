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

public partial class AddSupplierWindow : Window
{
    private readonly Pim pim;

    public AddSupplierWindow ( Pim pim )
    {
        InitializeComponent ();
        this.pim = pim;

    }

    public async void DoneButtonHandler ( Object? sender, RoutedEventArgs args )
    {
        ResetControlBorders ();
        try
        {
            string name = GetName();
            string phoneNumber = GetPhoneNumber();

            if ( string.IsNullOrEmpty ( name ) )
                throw new InvalidTextBoxException ( "Supplier name cannot be empty", SupplierNameBox );

            if ( string.IsNullOrWhiteSpace ( phoneNumber ) || !IsValidPhoneNumber ( phoneNumber ) )
                throw new InvalidTextBoxException ( "Invalid phone number", PhoneNumberBox );

            Supplier supplier = new Supplier( name, phoneNumber);

            await pim.InsertSupplier ( supplier );
            UpdateSupplierList ();
            Reset ();
        }
        catch ( InvalidTextBoxException e )
        {
            e.Textbox.Text = string.Empty;
            e.Textbox.BorderBrush = Brushes.Red;
        }

        catch ( InvalidNumericUpDownException e )
        {
            e.NumericUpDown.BorderBrush = Brushes.Red;
        }

    }

    public void CancelButtonHandler ( Object? sender, RoutedEventArgs args )
    {
        Close ();
    }

    private async void UpdateSupplierList ()
    {
        SupplierGrid.ItemsSource = null;  // Clear old data

        Supplier[] suppliers = await pim.GetAllSuppliers();  // Await the async method
        SupplierGrid.ItemsSource = suppliers.ToList ();  // Convert array to List
    }
}