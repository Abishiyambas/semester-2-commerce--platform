using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace DAM.Views.CRUD;

public partial class Create : Window
{
    public Create()
    {
        InitializeComponent();
        Width = 450;
        Height = 300;
    }
    
    private async void Choose_File(object? sender, RoutedEventArgs e)
    {
        try
        {
            var topLevel = GetTopLevel(this);
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select Image",
                AllowMultiple = false,
                FileTypeFilter = [FilePickerFileTypes.ImagePng, FilePickerFileTypes.Pdf, new FilePickerFileType(".mp4")]
            });

            Uri path = files[0].Path;
            
            string imagePath = path.ToString();
            string type = imagePath.Substring(imagePath.LastIndexOf('.'));
            string filename = imagePath.Substring(imagePath.LastIndexOf('/') + 1);
            string fileDestination = "";

            switch (type)
            {
                case ".png":
                    fileDestination = $"{Directory.GetCurrentDirectory()}/Assets/Picture/{filename}";
                    break;
                case ".pdf":
                    fileDestination = $"{Directory.GetCurrentDirectory()}/Assets/PDF/{filename}";
                    break;
                case ".mp4":
                    fileDestination = $"{Directory.GetCurrentDirectory()}/Assets/Films/{filename}";
                    break;
                default:
                    Console.WriteLine("Unknown type");
                    break;
            }

            if (OperatingSystem.IsWindows())
            {
                fileDestination.Replace('/', '\\');
            }
            
            PathBox.Text = imagePath;
            TypeBox.Text = imagePath.Substring(imagePath.LastIndexOf('.'));
            NameBox.Text = imagePath.Substring(imagePath.LastIndexOf('/') + 1);
            if (OperatingSystem.IsWindows())
            {
                NameBox.Text = imagePath.Substring(imagePath.LastIndexOf('\\') + 1);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
    
    private void Button_CreateFile(object? sender, RoutedEventArgs e)
    {
        try
        {
            string imagePath = PathBox.Text!.Substring(PathBox.Text.IndexOf(':') + 1);
            int productId = Int32.Parse(UuidBox.Text!);
            string name = NameBox.Text!;
            string type = imagePath.Substring(imagePath.LastIndexOf('.'));
            string filename = imagePath.Substring(imagePath.LastIndexOf('/') + 1);
            if (OperatingSystem.IsWindows())
            {
                filename = imagePath.Substring(imagePath.LastIndexOf('\\') + 1);
            }

            string fileDestination = ""; // TODO unit test

            switch (type)
            {
                case ".png":
                    fileDestination = $"{Directory.GetCurrentDirectory()}/assets/picture";
                    break;
                case ".pdf":
                    fileDestination = $"{Directory.GetCurrentDirectory()}/assets/pdf";
                    break;
                case ".mp4":
                    fileDestination = $"{Directory.GetCurrentDirectory()}/assets/films";
                    break;
                default:
                    Console.WriteLine("Unknown type");
                    break;
            }

            if (OperatingSystem.IsWindows())
            {
                fileDestination.Replace('/', '\\');
            }
            
            File.Copy(imagePath, Path.Combine(fileDestination, filename));
            UuidBox.Text = "";
            NameBox.Text = "";
            TypeBox.Text = "";
            PathBox.Text = "";
            MainWindow._databaseConnection.CreateFile(productId, name, type, Path.Combine(fileDestination, filename));
            Scaling.Scale(Path.Combine(fileDestination, filename));
        }
        catch (Exception ex) // TODO delete from table if exception is cast (test new code execution sequence)
        {
            Console.WriteLine(ex.Message);
            
        }
    }
    
        // TODO unit test, large files(edge case), GUI, data population
        // TODO integration test, input from GUI makes changes in database
}