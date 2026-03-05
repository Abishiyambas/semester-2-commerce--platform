using Avalonia.Controls;
using Avalonia.Interactivity;
using PIM_Logic;
using System;

namespace PIM_Presentation;

public partial class ProductDetailsWindow : Window
{
    public ProductDetailsWindow(Product product)
    {
        InitializeComponent();

        if (NameText == null || PriceText == null || CategoryText == null ||
            SupplierText == null || RatingText == null || ActiveText == null ||
            StockText == null || DescriptionText == null || CreationDateText == null ||
            LastUpdatedText == null || SkuText == null)
        {
            throw new Exception("Et eller flere kontrol-elementer fra XAML kunne ikke initialiseres.");
        }

        NameText.Text = product?.Name ?? "N/A";
        PriceText.Text = $"{product?.Price ?? 0} DKK";
        SkuText.Text = product?.GetSku () ?? "N/A";
        CategoryText.Text = product?.GetCategory()?.Name ?? "N/A";
        SupplierText.Text = product?.GetSupplier()?.Name ?? "N/A";
        RatingText.Text = ((int)(product?.GetAverageReviewRating() ?? 0)).ToString();
        ActiveText.Text = product?.Status == true ? "✅" : "⛔";
        StockText.Text = product?.Current_stock.ToString() ?? "N/A";
        DescriptionText.Text = string.IsNullOrWhiteSpace(product?.Description)
            ? "No description available."
            : product.Description;

        CreationDateText.Text = product != null && product.Created_at > DateTime.MinValue
            ? $"Creation date: {product.Created_at:dd/MM/yyyy - HH:mm:ss}"
            : "Creation date: Not available";

        LastUpdatedText.Text = product != null && product.Updated_at > DateTime.MinValue
            ? $"Last updated date: {product.Updated_at:dd/MM/yyyy - HH:mm:ss}"
            : "Last updated date: Not available";

    }

    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}