using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using PIM_Exceptions;
using PIM_Logic;
namespace PIM_Presentation;

public partial class EditEmployeeWindow : Window
{
    private readonly Pim pim;
    private Employee employee;


    public EditEmployeeWindow(Pim pim, Employee employeeToEdit)
    {
        InitializeComponent();
        this.pim = pim;
        employee = employeeToEdit;
        LoadEmployeeData();
        
        if ( EmployeeManager.ActiveEmployee != null && EmployeeManager.ActiveEmployee.Employee_id == employee.Employee_id)
        {
            AuthorizedCheckBox.IsEnabled = false;
            AuthorizedCheckBox.IsChecked = employee.Authorized;
        }
        else
        {
            AuthorizedCheckBox.IsEnabled = true;
            AuthorizedCheckBox.IsChecked = employee.Authorized;
        }
    }

    private void LoadEmployeeData()
    {
        EmployeeNameBox.Text = employee.Name;
        AuthorizedCheckBox.IsChecked = employee.Authorized;
    }
    
    public void ShowOldPasswordHandler ( object? sender, RoutedEventArgs args )
    {
        if ( sender is CheckBox checkBox )
        {
            if ( checkBox.IsChecked == false )
            {
                OldPasswordTextBox.PasswordChar = '*';
            }
            else
            {
                OldPasswordTextBox.PasswordChar = new char ();
            }
        }
    }
    
    public void ShowNewPasswordHandler ( object? sender, RoutedEventArgs args )
    {
        if ( sender is CheckBox checkBox )
        {
            if ( checkBox.IsChecked == false )
            {
                NewPasswordTextBox.PasswordChar = '*';
            }
            else
            {
                NewPasswordTextBox.PasswordChar = new char ();
            }
        }
    }
    
    public void ShowConfirmNewPasswordHandler ( object? sender, RoutedEventArgs args )
    {
        if ( sender is CheckBox checkBox )
        {
            if ( checkBox.IsChecked == false )
            {
                ConfirmNewPasswordTextBox.PasswordChar = '*';
            }
            else
            {
                ConfirmNewPasswordTextBox.PasswordChar = new char ();
            }
        }
    }

    public async void DoneButtonHandler(object? sender, RoutedEventArgs args)
    {
        ResetControlBorders();
        try
        {
            employee.Name = GetName();
            employee.Password = GetNewPassword(employee);
            employee.Authorized = GetAuthorization();
        
            await pim.UpdateEmployee(employee);
        
            StatusTextBox.Foreground = Brushes.Green;
            StatusTextBox.Text = "Employee edited successfully.";
        
            Reset();
        }
        catch (InvalidTextBoxException e)
        {
            e.Textbox.Text = string.Empty;
            if (e.Textbox.Name == "PasswordTextBox")
            {
                ConfirmNewPasswordTextBox.Text = string.Empty;
            }
            e.Textbox.BorderBrush = Brushes.Red;
        
            StatusTextBox.Foreground = Brushes.Red;
            StatusTextBox.Text = $"{e.Message}";
        }
    }

    
    public void CancelButtonHandler(Object? sender, RoutedEventArgs args)
    {
        Close ();
    }
}