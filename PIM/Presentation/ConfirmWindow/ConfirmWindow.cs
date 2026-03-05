using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PIM_Presentation;

public partial class ConfirmWindow : Window
{
    public ConfirmWindow()
    {
        InitializeComponent();
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false); // Brugeren fortrød
    }

    private void Confirm_Click(object? sender, RoutedEventArgs e)
    {
        Close(true); // Brugeren bekræftede
    }
}