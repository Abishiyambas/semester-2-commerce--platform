using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PIM_Data;
using PIM_Logic;

namespace PIM_Presentation;

public partial class MainWindow : Window
{
    public MainWindow ()
    {
        InitializeComponent();

        DatabaseUtility databaseUtility = new DatabaseUtility();
        EmployeeManager employeeManager = new EmployeeManager(databaseUtility);
        Pim pim = new Pim(databaseUtility);

        SetContent ( new LogInPage ( SetContent, pim, employeeManager ) ); 
    }

    public void SetContent(Control? control)
    {
        try
        {
            Content = control;
        }
        catch(Exception e)
        { }   
    }
}