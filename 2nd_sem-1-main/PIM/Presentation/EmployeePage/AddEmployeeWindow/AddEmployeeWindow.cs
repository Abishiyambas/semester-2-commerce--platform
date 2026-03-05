using Avalonia.Controls;
using Avalonia.Media;
using NUnit.Framework;
using PIM_Exceptions;
using PIM_Logic;

namespace PIM_Presentation;

public partial class AddEmployeeWindow : Window
{ 
    private string GetName()
    {
        if (string.IsNullOrWhiteSpace( EmployeeNameBox.Text ))
            throw new InvalidTextBoxException ( "Employee Name does not contain a value", EmployeeNameBox );
        return EmployeeNameBox.Text;
    }

    private string GetPassword()
    {
        if (string.IsNullOrWhiteSpace(PasswordTextBox.Text))
        {
            throw new InvalidTextBoxException("Password does not contain a value", PasswordTextBox);
        }

        if (PasswordTextBox.Text.Length < 8 && !string.IsNullOrWhiteSpace(PasswordTextBox.Text))
        {
            throw new InvalidTextBoxException("Password is less than 8 characters long", PasswordTextBox);
        }

        if (PasswordTextBox.Text != ConfirmPasswordTextBox.Text)
        {
            throw new InvalidTextBoxException("Passwords do not match", ConfirmPasswordTextBox);
        }

        return PasswordTextBox.Text;
    }

    public bool GetAuthorization()
    {
        return AuthorizedCheckBox.IsChecked ?? false;
    }
    
    private void Reset ()
    {
        EmployeeNameBox.Text = string.Empty;
        PasswordTextBox.Text = string.Empty;
        ConfirmPasswordTextBox.Text = string.Empty;
    }
    
    private void ResetControlBorders ()
    {
        EmployeeNameBox.BorderBrush = Brushes.Black;
        PasswordTextBox.BorderBrush = Brushes.Black;
        ConfirmPasswordTextBox.BorderBrush = Brushes.Black;
    }
}