using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System.IO;
using PIM_Logic;

namespace PIM_Presentation;

public partial class EditProductWindow : Window
{
    private readonly Pim pim;
    private Product product;
    private Supplier[] _suppliers;
    private Category[] _categories;
    
    public EditProductWindow(Pim pim, Product productToEdit)
    {
        InitializeComponent();
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
        var iconPath = Path.Combine(projectRoot, "PIM", "Content", "edit-icon.png");

        if (File.Exists(iconPath))
        {
            var editImage = new Image
            {
                Source = new Bitmap(iconPath),
                Stretch = Stretch.Uniform,
                Width = 20,
                Height = 20
            };

            EditCategoryButton.Content = editImage;

            var editImage2 = new Image
            {
                Source = new Bitmap(iconPath),
                Stretch = Stretch.Uniform,
                Width = 20,
                Height = 20
            };

            EditSupplierButton.Content = editImage2;
        }
        else
        {
            Console.WriteLine("Icon not found at: " + iconPath);
        }

        
        this.pim = pim;
        product = productToEdit;

        Width = 900;
        Height = 450;

        SetupComboBoxes();
        LoadProductData();
    }

    private async void SetupComboBoxes()
    {
        _categories = await pim.GetAllCategories();
        _suppliers = await pim.GetAllSuppliers();

        foreach (Category category in _categories)
        {
            ProductCategoryComboBox.Items.Add(category.Name);
        }
        foreach (Supplier supplier in _suppliers)
        {
            ProductSupplierComboBox.Items.Add(supplier.Name);
        }
    
        ProductCategoryComboBox.SelectedItem = _categories.FirstOrDefault(c => c.Category_id == product.Category_id)?.Name;
        ProductSupplierComboBox.SelectedItem = _suppliers.FirstOrDefault(s => s.Supplier_id == product.Supplier_id)?.Name;
    }


    private void LoadProductData()
    {
        ProductNameBox.Text = product.Name;
        ProductPriceBox.Text = product.Price.ToString("F2");
        ProductDescriptionBox.Text = product.Description;
        MaxStockNumericUpDown.Value = product.Max_stock;
        StatusCheckBox.IsChecked = product.Status;

        DiscountCheckBox.IsChecked = false;
        StartDatePicker.SelectedDate = DateTime.Today;
        EndDatePicker.SelectedDate = DateTime.Today.AddDays(7);
    }

    private async void UpdateButtonHandler(object? sender, RoutedEventArgs e)
    {
        product.Name = ProductNameBox.Text;
    
        if (decimal.TryParse(ProductPriceBox.Text, out decimal newPrice))
        {
            product.Price = newPrice;
        }
        else
        {
            Console.WriteLine("Invalid price entered.");
        }
    
        product.Description = ProductDescriptionBox.Text;
        product.Max_stock = Convert.ToInt32(MaxStockNumericUpDown.Value);
        product.Updated_at = DateTime.Now;
        
        product.Status = StatusCheckBox.IsChecked ?? false;
    
        var selectedCategoryName = ProductCategoryComboBox.SelectedItem?.ToString();
        if (!string.IsNullOrEmpty(selectedCategoryName))
        {
            var selectedCategory = _categories.FirstOrDefault(c => c.Name == selectedCategoryName);
            if (selectedCategory != null)
            {
                product.Category_id = selectedCategory.Category_id; 
            }
        }
    
        var selectedSupplierName = ProductSupplierComboBox.SelectedItem?.ToString();
        if (!string.IsNullOrEmpty(selectedSupplierName))
        {
            var selectedSupplier = _suppliers.FirstOrDefault(s => s.Name == selectedSupplierName);
            if (selectedSupplier != null)
            {
                product.Supplier_id = selectedSupplier.Supplier_id;
            }
        }
    
        await pim.UpdateProduct(product);
    
        Console.WriteLine("Product updated.");
        Close();
    }

    private void CancelButtonHandler(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("Cancel clicked");
        Close();
    }
    
    private void DiscountCheckBox_Checked(object? sender, RoutedEventArgs e)
    {
        DiscountDatesPanel.IsVisible = true;
    }

    private void DiscountCheckBox_Unchecked(object? sender, RoutedEventArgs e)
    {
        DiscountDatesPanel.IsVisible = false;
    }    
}