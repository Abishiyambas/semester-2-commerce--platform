using System;
using SHOP.Classes;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;
using SHOP.ViewModels;

namespace SHOP.Views;

public partial class LogInView : UserControl
{
    public LogInViewModel? ViewModel => this.DataContext as LogInViewModel;
    public LogInView()
    {
        InitializeComponent();
    }
    
    private void Login(object? sender, RoutedEventArgs e)
    {
        var user = PrivateCustomer.LogIn(UserName.Text, Password.Text);
        if (user != null)
        {
            MainViewModel.Instance.UserLoggedIn = true;
            ViewModel.GoToHomeCommand.Execute(user);
            Console.WriteLine(MainViewModel.Instance.UserLoggedIn);
            Console.WriteLine("Login successful");
      
        }
        else
        {
            UserName.Text = "Password or username is invalid";
            
            Console.WriteLine("Password or username is invalid");
        }
    }
    
    private void ClearOnFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is TextBox tb)
        {
            tb.Clear();
        }
    }
}