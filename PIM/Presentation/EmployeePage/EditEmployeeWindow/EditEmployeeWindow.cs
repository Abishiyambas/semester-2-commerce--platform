using Avalonia.Controls;
using Avalonia.Media;
using NUnit.Framework;
using PIM_Exceptions;
using PIM_Logic;

namespace PIM_Presentation;

public partial class EditEmployeeWindow : Window
{
    private string GetName()
    {
        if (string.IsNullOrWhiteSpace( EmployeeNameBox.Text ))
            throw new InvalidTextBoxException ( "Employee Name does not contain a value", EmployeeNameBox );
        return EmployeeNameBox.Text;
    }

    private string GetNewPassword(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(OldPasswordTextBox.Text) && string.IsNullOrWhiteSpace(NewPasswordTextBox.Text))
        {
            return employee.Password;
        }
        
        if (string.IsNullOrWhiteSpace(OldPasswordTextBox.Text))
        {
            throw new InvalidTextBoxException("Old Password does not contain a value", OldPasswordTextBox);
        }

        if (OldPasswordTextBox.Text != employee.Password)
        {
            throw new InvalidTextBoxException("Old Password is incorrect", OldPasswordTextBox);
        }

        if (string.IsNullOrWhiteSpace(NewPasswordTextBox.Text))
        {
            throw new InvalidTextBoxException("New Password does not contain a value", OldPasswordTextBox);
        }
        
        if (NewPasswordTextBox.Text.Length < 8 && !string.IsNullOrWhiteSpace(NewPasswordTextBox.Text))
        {
            throw new InvalidTextBoxException("New Password is less than 8 characters long", NewPasswordTextBox);
        }

        if (NewPasswordTextBox.Text != ConfirmNewPasswordTextBox.Text)
        {
            throw new InvalidTextBoxException("Passwords do not match", ConfirmNewPasswordTextBox);
        }

        return NewPasswordTextBox.Text;
    }

    public bool GetAuthorization()
    {
        return AuthorizedCheckBox.IsChecked ?? false;
    }
    
    private void Reset ()
    {
        EmployeeNameBox.Text = string.Empty;
        OldPasswordTextBox.Text = string.Empty;
        NewPasswordTextBox.Text = string.Empty;
        ConfirmNewPasswordTextBox.Text = string.Empty;
    }
    
    private void ResetControlBorders ()
    {
        EmployeeNameBox.BorderBrush = Brushes.Black;
        OldPasswordTextBox.BorderBrush = Brushes.Black;

        NewPasswordTextBox.BorderBrush = Brushes.Black;
        ConfirmNewPasswordTextBox.BorderBrush = Brushes.Black;
    }
}