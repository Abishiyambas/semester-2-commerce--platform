using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace DAM.Views.CRUD;

public partial class Update : Window {
    public Update() {
        InitializeComponent();
        Width = 450;
        Height = 300;
    }
    
    private async void Button_GetFile(object? sender, RoutedEventArgs e)
    {
        List<string> imageData = new();
        try
        {
            Uri path;
            var topLevel = GetTopLevel(this);
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select File",
                AllowMultiple = false
            });
            
            path = files[0].Path;
            string subPath = path.ToString();

            PathBox.Text = subPath.Substring(subPath.LastIndexOf(':') + 3);
            
            if (PathBox.Text != null)
            {
                imageData = MainWindow._databaseConnection.GetFileData(PathBox.Text.Substring(PathBox.Text.LastIndexOf('/') + 1));
            }
            
            UuidBox.Text = imageData[0];
            NameBox.Text = imageData[1];
            TypeBox.Text = imageData[2];
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            //throw new NoNullAllowedException("Need a valid file path"); // TODO unit test
            // TODO need proper error handling
        }
        // fetch table information on image and display in textboxes
    }
    
    private void Button_UpdateFile(object? sender, RoutedEventArgs e)
    {
        // When images is fetched, table information is also fetched.
        // If a desire to change information, change in cell and methods updates all information
        
        int product_id = Convert.ToInt32(UuidBox.Text);
        string name = NameBox.Text;
        string type = TypeBox.Text;
        string path = PathBox.Text;
        
        MainWindow._databaseConnection.UpdateFile(product_id, name, type, path); // TODO unit test
    }
}