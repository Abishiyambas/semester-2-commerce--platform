using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;

namespace SHOP.Classes;
//Hej venner
public class ImageHandler
{
    // Method to load an image into a specified control based on its name
    public void LoadImage(Window parentControl, string controlName, string imageUri)
    {
        try        {
                       // Find the Image control by name within the parent window (MainWindow).
                       // The parentControl is the Window (MainWindow) from where this method is called.
                       var imageControl = parentControl.FindControl<Image>(controlName);
           
                       // Check if the Image control is found.
                       if (imageControl == null)
                       {
                           // If the control is not found, log an error and exit the method.
                           Console.WriteLine($"Error: Image control with name '{controlName}' not found.");
                           return;
                       }
           
                       // Load the image from the resource using the URI string provided.
                       // The 'imageUri' string should be a valid URI to the image resource.
                       var uri = new Uri(imageUri);
           
                       // Open the resource as a stream using Avalonia's AssetLoader.
                       var stream = AssetLoader.Open(uri);
           
                       // Set the loaded image stream as the source for the Image control.
                       imageControl.Source = new Bitmap(stream);
        }

        catch (Exception ex)
        {
            // If any error occurs (e.g., invalid URI, resource not found, etc.), log the error message.
            Console.WriteLine($"Error loading image for {controlName}: {ex.Message}");
        }
    }
}