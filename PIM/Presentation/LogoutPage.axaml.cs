using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PIM_Logic;

namespace PIM_Presentation;

public partial class LogoutPage : Page
{ 
    Action<Control> SetWindowContent;
    Control loginWindow;

    public LogoutPage(Action<Control> SetWindowContent, Control loginWindow)
    {
        InitializeComponent();
        this.loginWindow = loginWindow;
        this.SetWindowContent = SetWindowContent;
        ActiveEmployeeTextBlock.Text = $"Goodbye {EmployeeManager.ActiveEmployee.Name}";
    }

    public void LogOutHandler( object? sender, RoutedEventArgs e )
    {
        EmployeeManager.ActiveEmployee = null;
        SetWindowContent( loginWindow );
    }


}