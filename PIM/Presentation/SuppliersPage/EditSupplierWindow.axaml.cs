using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System.IO;
using PIM_Logic;

namespace PIM_Presentation;

public partial class EditSupplierWindow : Window
{
    private readonly Pim pim;
    private Supplier supplier;

    public EditSupplierWindow(Pim pim, Supplier supplierToEdit)
    {
        InitializeComponent();
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
        var iconPath = Path.Combine(projectRoot, "PIM", "Content", "edit-icon.png");

        this.pim = pim;
        supplier = supplierToEdit;
        LoudSupplierData();
    }

    private void LoudSupplierData()
    {
        SupplierRow.Text = supplier.Name;
        PhoneRow.Text = supplier.Phone;
    }

    private async void UpdateButtonHandler(object sender, RoutedEventArgs e)
    {
        supplier.Name = SupplierRow.Text;
        supplier.Phone = PhoneRow.Text;

        await pim.UpdateSupplier(supplier);
        Console.WriteLine("Supplier updated.");
        Close();
    }
    
    private void CancelButtonHandler(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("Cancel clicked");
        Close();
    }
}