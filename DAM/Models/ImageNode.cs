using Avalonia.Media.Imaging;

namespace DAM.Models;

public class ImageNode
{
    public Bitmap image { get; set; }
    public string name { get; set; }
    
    public ImageNode(string name, Bitmap image)
    {
        this.name=name;
        this.image=image;
    }
}