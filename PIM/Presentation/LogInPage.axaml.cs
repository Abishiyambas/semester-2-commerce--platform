using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using PIM_Logic;
using System.Threading.Tasks;
namespace PIM_Presentation;

public partial class LogInPage : Page
{
    private readonly EmployeeManager employeeManager;
    private readonly Pim pim;
    Action<Control>? SetWindowContent;

    public LogInPage (Action<Control> SetContentAction, Pim pim, EmployeeManager employeeManager )
    {
        if ( !AppEnvironment.IsTestEnvironment)
        {
            this.SetWindowContent = SetContentAction;
        }
        InitializeComponent ();
        
        this.employeeManager = employeeManager;
        this.pim = pim;
    }

    public async void LogInHandler ( object? sender, RoutedEventArgs args )
    {
        if ( IdTextBox.Text == null || PasswordTextBox.Text == null )
        {
            Wrong ();
            return;
        }
        if ( await LogIn( IdTextBox.Text, PasswordTextBox.Text ) )
        {
            Correct ();
            SetWindowContent ( new MainTabControl ( SetWindowContent, pim, employeeManager ) );
        }
        else
        {
            Wrong ();
        }
    }

    public async Task<bool> LogIn(string id, string password )
    {
        if ( await employeeManager.DoesEmployeeExist ( id, password ) )
        {
            EmployeeManager.ActiveEmployee = await employeeManager.GetAccount ( id, password );   
            return true;
        }
        else
        {
            return false;
        }
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

    private void Correct()
    {
        IdTextBox.Text = string.Empty;
        PasswordTextBox.Text = string.Empty;
        IdTextBox.BorderBrush = Brushes.Green;
        PasswordTextBox.BorderBrush = Brushes.Green;
    }

    private void Wrong()
    {
        IdTextBox.BorderBrush = Brushes.Red;
        PasswordTextBox.BorderBrush = Brushes.Red;
        return;
    }
}