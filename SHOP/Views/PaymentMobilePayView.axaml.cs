using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SHOP.Classes;
using SHOP.ViewModels;
using Tmds.DBus.Protocol;

namespace SHOP.Views;

public partial class PaymentMobilePayView : UserControl
{
    public PaymentMobilePayView()
    {
        InitializeComponent();
    }
    
    private void ConfirmButton_OnClick(object? sender, RoutedEventArgs e)
    {
        bool isValid = true;
        if (string.IsNullOrWhiteSpace(TelephoneNumberTextBox.Text)) 
        {
            TelephoneNumberLabel.Text = " Telephone number is required";
            isValid = false;
        }
        else if (!TelephoneNumberTextBox.Text.All(char.IsDigit) || TelephoneNumberTextBox.Text.Length != 8)
        {
            TelephoneNumberLabel.Text = "Telephone number is not valid";
            isValid = false;
        }
        else
        {
            isValid = true;
            TelephoneNumberLabel.Text = "";
        }
        if (isValid)
        {
            if (this.DataContext is PaymentMobilePayViewModel viewModel)
            {
                PrivateCustomer? user = PrivateCustomer.LoggedInCustomer;

                if (!MainViewModel.Instance.UserLoggedIn)
                {
                    user = new PrivateCustomer();
                    user = user.RegisterNoLogin(viewModel.Name, viewModel.Email, viewModel.Address, viewModel.PostalCode, viewModel.City, viewModel.Country, viewModel.TelephoneNumber);

                    if (user == null)
                    {
                        Console.WriteLine("Failed to register user");
                        return;
                    }
                }

                Order order = new Order(user, user.Address, user.Zip);
                order.AddOrderToDatabase(user, ListInitializer.Instance.CartItemList, viewModel.Address, viewModel.PostalCode);
                viewModel.GoToConfirmOrderPageCommand.Execute(null);
            }
        }
    }
}