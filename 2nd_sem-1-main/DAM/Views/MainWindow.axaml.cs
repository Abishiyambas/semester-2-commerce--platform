using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DAM.Models;
using DAM.Services;
using DAM.ViewModels;
using DAM.Views.CRUD;

namespace DAM.Views;

public partial class MainWindow : Window
{
    public static readonly PostGresContext _databaseConnection = new(); // TODO Do we make it public, so that other windows can use it?
    
    public MainWindow() {
        InitializeComponent();
        Width = 1200;
        Height = 675; //16:9
        CanResize = false;
    }
    
    private void CreateWindow(Object sender, RoutedEventArgs e) {
        Window create = new Create();
        create.Show();
    }
    
    private void ReadWindow(Object sender, RoutedEventArgs e) {
        Window read = new Read();
        read.Show();
    }
    
    private void UpdateWindow(Object sender, RoutedEventArgs e) {
        Window update = new Update();
        update.Show();
    }
    
    private void DeleteWindow(Object sender, RoutedEventArgs e) {
        Window delete = new Delete();
        delete.Show();
    }
}