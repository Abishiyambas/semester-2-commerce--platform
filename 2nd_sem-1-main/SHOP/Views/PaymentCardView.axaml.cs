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

public partial class PaymentCardView : UserControl
{
    public CheckoutViewModel CheckoutViewModel { get; set; }
    public PaymentCardView()
    {
        InitializeComponent();
    }

    private void PayButton_OnClick(object? sender, RoutedEventArgs e)
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(CardNumberTextBox.Text))
        {
            CardNumberLabel.Content = "Cardnumber is required";
            isValid = false;
        }
        else if (!CardNumberTextBox.Text.All(char.IsDigit) || CardNumberTextBox.Text.Length != 16)
        {
            CardNumberLabel.Content = "Carnumber is not valid";
            isValid = false;
        }
        else
        {
            CardNumberLabel.Content = "";
        }

        if (string.IsNullOrWhiteSpace(MonthTextBox.Text) || string.IsNullOrWhiteSpace(YearTextBox.Text))
        {
            ExpiryDateLabel.Content = "Date is required";
            isValid = false;
        }
        else if ((!MonthTextBox.Text.All(char.IsDigit) || MonthTextBox.Text.Length != 2) ||
                 (!YearTextBox.Text.All(char.IsDigit) || YearTextBox.Text.Length != 2))
        {
            ExpiryDateLabel.Content = "Date is not valid";
            isValid = false;
        }
        else
        {
            int month = int.Parse(MonthTextBox.Text);
            int year = int.Parse(YearTextBox.Text) + 2000; // fx "25" bliver til 2025

            if (month < 1 || month > 12)
            {
                ExpiryDateLabel.Content = "Invalid month";
                isValid = false;
            }
            else
            {
                DateTime expiryDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1); // Sidste dag i måneden
                DateTime today = DateTime.Today;

                if (expiryDate < today)
                {
                    ExpiryDateLabel.Content = "Card is expired";
                    isValid = false;
                }
                else
                {
                    ExpiryDateLabel.Content = "";
                }
            }
        }


        if (string.IsNullOrWhiteSpace(CvvCvdTextBox.Text))
        {
            CvvCvdLabel.Content = "CVV/CVD number is required";
            isValid = false;
        }
        else if (!CvvCvdTextBox.Text.All(char.IsDigit) || CvvCvdTextBox.Text.Length != 3)
        {
            CvvCvdLabel.Content = "CVV/CVD is not valid";
            isValid = false;
        }
        else
        {
            CvvCvdLabel.Content = "";
        }
       
        if (isValid)
        {
            if (this.DataContext is PaymentCardViewModel viewModel)
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