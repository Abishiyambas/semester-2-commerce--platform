using System;
using Avalonia.Controls;
using Avalonia.Media;
using System.Text.RegularExpressions;

using PIM_Exceptions;
namespace PIM_Presentation;

public partial class AddSupplierWindow : Window
{
    private string GetName()
    {
        if ( string.IsNullOrWhiteSpace ( SupplierNameBox.Text ) )
            throw new InvalidTextBoxException ( "SupplierNameBox does not contain a value", SupplierNameBox );
        return SupplierNameBox.Text;
    }

    public string GetPhoneNumber()
    {
        if (PhoneNumberBox == null)
            throw new InvalidTextBoxException("PhoneNumberTextBox cannot be null", PhoneNumberBox);
        
        if (string.IsNullOrWhiteSpace (PhoneNumberBox.Text))
            throw new FormatException("Phone number cannot be empty");
        
        if(!IsValidPhoneNumber (PhoneNumberBox.Text)) 
            throw new FormatException("Invalid phone number format. Must be numeric.");
        
        return PhoneNumberBox.Text;
    }
    private bool IsValidPhoneNumber(string phone)
    {
        return Regex.IsMatch(phone, @"^\d{7,15}$");  // Adjust the pattern as needed
    }

    private void Reset()
    {
        SupplierNameBox.Text = string.Empty;
        PhoneNumberBox.Text = string.Empty;
    }

    private void ResetControlBorders()
    {
        SupplierNameBox.BorderBrush = Brushes.Black;
        PhoneNumberBox.BorderBrush = Brushes.Black;
    }

   
}