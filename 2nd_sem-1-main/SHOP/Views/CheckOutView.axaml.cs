using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Dapper;
using Npgsql;
using SHOP.ViewModels;

namespace SHOP.Views;

public partial class CheckoutView : UserControl
{
    public CheckoutViewModel CheckoutViewModelen { get; set; }
    public CheckoutView()
    {
        InitializeComponent();
    }
    
    private void Button_Overview_OnClick(object? sender, RoutedEventArgs e)
{
    bool isValid = true;

    if (string.IsNullOrWhiteSpace(NameTextBox.Text))
    {
        NameErrorLabel.Content = "Name is required";
        isValid = false;
    }
    else
    {
        NameErrorLabel.Content = "";
    }

    if (string.IsNullOrWhiteSpace(AddressTextBox.Text))
    {
        AddressErrorLabel.Content = "Address is required";
        isValid = false;
    }
    else
    {
        AddressErrorLabel.Content = "";
    }

    if (string.IsNullOrWhiteSpace(PostalCodeTextBox.Text) || PostalCodeTextBox.Text.Length != 4 || !int.TryParse(PostalCodeTextBox.Text, out _))
    {
        PostalCodeErrorLabel.Content = "Postal code must be 4 digits";
        isValid = false;
    }
    else
    {
        PostalCodeErrorLabel.Content = "";
    }
    
    
    if (string.IsNullOrWhiteSpace(CityTextBox.Text))
    {
        CityErrorLabel.Content = "Town is required";
        isValid = false;
    }
    else
    {
        CityErrorLabel.Content = "";
    }

    if (string.IsNullOrWhiteSpace(CountryTextBox.Text))
    {
        CountryErrorLabel.Content = "Country is required";
        isValid = false;
    }
    else
    {
        CountryErrorLabel.Content = "";
    }

    if (string.IsNullOrWhiteSpace(TelephoneTextBox.Text) || TelephoneTextBox.Text.Length != 8 || !int.TryParse(TelephoneTextBox.Text, out _))
    {
        TelephoneErrorLabel.Content = "Telephone must be 8 digits";
        isValid = false;
    }
    else
    {
        TelephoneErrorLabel.Content = "";
    }

    var emailRegEx = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
    {
        EmailErrorLabel.Content = "Email is required";
        isValid = false;
    }
    else
    {
        try
        {
            var addr = new MailAddress(EmailTextBox.Text);
            // Tjek med regex for at sikre omæne som .dk, .com osv.
            if (!Regex.IsMatch(EmailTextBox.Text, emailRegEx, RegexOptions.IgnoreCase))
            {
                EmailErrorLabel.Content = "Email must include a domain (.dk or .com)";
                isValid = false;
            }
            else
            {
                EmailErrorLabel.Content = "";
            }
        }
        catch
        {
            EmailErrorLabel.Content = "Email format is invalid";
            isValid = false;
        }
    }
    if (ZipCheck()) return;

    if (!HomeDeliveryButton.IsChecked.GetValueOrDefault() &&
        !ParcelShopDeliveryButton.IsChecked.GetValueOrDefault() &&
        !CollectInStoreButton.IsChecked.GetValueOrDefault())
    {
        RadioButtonIsNotChecked.Content = "Choose a delivery option";
        isValid = false;
    }
    
    else
    {
        RadioButtonIsNotChecked.Content = "";
    }

    if (isValid)
    {
        if (this.DataContext is CheckoutViewModel viewModel)
        {
            viewModel.GoToOverviewCommand.Execute(null);
            
        }
    }
}

    private void RadioButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (this.DataContext is CheckoutViewModel viewModel)
        {
            if (sender == HomeDeliveryButton)
            {
                viewModel.DeliveryPriceChange(50); 
            }

            else if (sender == ParcelShopDeliveryButton)
            {
                viewModel.DeliveryPriceChange(40);
            }
            
            else if (sender == CollectInStoreButton)
            {
                viewModel.DeliveryPriceChange(0);
            }
            RadioButtonIsNotChecked.Content = null;
        }
    }
    
    private bool ZipCheck()
    {
        // Tjek om zip og city findes i databasen
        using var con = new NpgsqlConnection(new Credentials.Credentials().Database);
        con.Open();

        string zipCityQuery = "SELECT COUNT(1) FROM zip WHERE zip = @Zip AND city = @City";
        int count = con.ExecuteScalar<int>(zipCityQuery, new { Zip = PostalCodeTextBox.Text, City = CityTextBox.Text });

        if (count == 0)
        {
            PostalCodeErrorLabel.Content = "Postal Code or city is invalid";
            Console.WriteLine("Zip and City combination not found in database.");
            return true;
        }

        return false;
    }
    
    private void ClearOnFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is TextBox tb)
        {
            tb.Clear();
        }
    }
}