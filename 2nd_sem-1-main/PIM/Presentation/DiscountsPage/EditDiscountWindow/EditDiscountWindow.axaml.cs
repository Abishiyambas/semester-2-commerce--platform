using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using PIM_Logic;

namespace PIM_Presentation;

public partial class EditDiscountWindow : Window
{
    private readonly Pim _pim;
    private readonly Discount _discount;

    public EditDiscountWindow(Pim pim, Discount discount, string targetName)
    {
        InitializeComponent();
        _pim = pim;
        _discount = discount;

        // Pre-fill fields
        DiscountIdTextBox.Text = discount.Discount_id.ToString();
        TargetTextBox.Text = targetName;
        DiscountPctBox.Text = discount.Discount_percentage.ToString();
        StartDatePicker.SelectedDate = new DateTimeOffset(discount.Start_date);
        EndDatePicker.SelectedDate = new DateTimeOffset(discount.End_date);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        // Validate percentage
        if (!int.TryParse(DiscountPctBox.Text, out var pct) || pct < 0 || pct > 100)
        {
            StatusTextBlock.Foreground = Brushes.Red;
            StatusTextBlock.Text = "Discount % must be 0–100";
            return;
        }

        var start = StartDatePicker.SelectedDate?.DateTime;
        var end = EndDatePicker.SelectedDate?.DateTime;

        if (start == null || end == null || end <= start)
        {
            StatusTextBlock.Foreground = Brushes.Red;
            StatusTextBlock.Text = "End date must be after start date";
            return;
        }

        // Update model
        _discount.Discount_percentage = pct;
        _discount.Start_date = start.Value.ToUniversalTime();
        _discount.End_date = end.Value.ToUniversalTime();

        // Persist
        await _pim.UpdateDiscount(_discount);

        StatusTextBlock.Foreground = Brushes.Green;
        StatusTextBlock.Text = "Discount updated successfully";
        Close(true);
    }
}