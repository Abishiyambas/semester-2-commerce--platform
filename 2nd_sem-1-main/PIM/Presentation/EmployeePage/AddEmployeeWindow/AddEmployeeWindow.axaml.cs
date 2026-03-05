using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using PIM_Exceptions;
using PIM_Logic;
namespace PIM_Presentation;

public partial class AddEmployeeWindow : Window
{
    private readonly Pim pim;
    private readonly Action<object?, RoutedEventArgs> refreshAction;


    public AddEmployeeWindow(Pim pim, Action<object?, RoutedEventArgs> refreshAction)
    {
        InitializeComponent();
        this.pim = pim;
        this.refreshAction = refreshAction;
    }
    
    public void ShowPasswordHandler ( object? sender, RoutedEventArgs args )
    {
        if ( sender is CheckBox checkBox )
        {
            if ( checkBox.IsChecked == false )
            {
                PasswordTextBox.PasswordChar = '*';
            }
            else
            {
                PasswordTextBox.PasswordChar = new char ();
            }
        }
    }
    
    public void ShowConfirmPasswordHandler ( object? sender, RoutedEventArgs args )
    {
        if ( sender is CheckBox checkBox )
        {
            if ( checkBox.IsChecked == false )
            {
                ConfirmPasswordTextBox.PasswordChar = '*';
            }
            else
            {
                ConfirmPasswordTextBox.PasswordChar = new char ();
            }
        }
    }

    public async void DoneButtonHandler(object? sender, RoutedEventArgs args)
    {
        ResetControlBorders();
        try
        {
            Employee employee = new Employee(
                GetName(), 
                GetPassword(),
                GetAuthorization()
            );
        
            await pim.InsertEmployee(employee);
        
            refreshAction.Invoke(null, null);
            StatusTextBox.Foreground = Brushes.Green;
            StatusTextBox.Text = "Employee added successfully.";
        
            Reset();
        }
        catch (InvalidTextBoxException e)
        {
            e.Textbox.Text = string.Empty;
            if (e.Textbox.Name == "PasswordTextBox")
            {
                ConfirmPasswordTextBox.Text = string.Empty;
            }
            e.Textbox.BorderBrush = Brushes.Red;
        
            StatusTextBox.Foreground = Brushes.Red;
            StatusTextBox.Text = $"{e.Message}";
        }
    }

    public void CancelButtonHandler(Object? sender, RoutedEventArgs args)
    {
        refreshAction.Invoke (null, null);
        Close ();
    }
}