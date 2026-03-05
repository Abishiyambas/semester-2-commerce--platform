using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SHOP.ViewModels;

namespace SHOP.Views;

public partial class OverviewView : UserControl
{
    public bool isValid = false;
    public OverviewViewModel? ViewModel => this.DataContext as OverviewViewModel;


    public OverviewView()
    {
        InitializeComponent();
    }

    private void AcceptTermsBox_OnChecked(object? sender, RoutedEventArgs e)
    {
        isValid = true;
        AcceptTermsErrorLabel.Content = "";
    }

    private void AcceptTermsBox_OnUnchecked(object? sender, RoutedEventArgs e)
    {
        isValid = false;
        AcceptTermsErrorLabel.Content = "You must accept the terms and conditions.";
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        bool isValid = true;

        if (AcceptTermsBox.IsChecked != true)
            {
                AcceptTermsErrorLabel.Content = "You must accept the terms and conditions.";
                isValid = false;
            }
            else
            {
                AcceptTermsErrorLabel.Content = "";
            }

            if (CardPaymentButton.IsChecked != true && MobilePayButton.IsChecked != true)
            {
                RadioButtonIsNotChecked.Content = "Choose payment";
                isValid = false;
            }
            else
            {
                RadioButtonIsNotChecked.Content = "";
            }

            // Hvis alt er gyldigt, så gå videre
            if (isValid)
            {
                if (CardPaymentButton.IsChecked == true)
                {
                    ViewModel?.GoToPaymentCardCommand.Execute(null);
                }
                else if (MobilePayButton.IsChecked == true)
                {
                    ViewModel?.GoToPaymentMobilPayCommand.Execute(null);
                }
            }
        }
    }