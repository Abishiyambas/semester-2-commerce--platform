using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Avalonia.Controls;
using Image = SixLabors.ImageSharp.Image;


namespace DAM;

public class Scaling
{
    
    public static void Scale(string path)
    {
        try
        {
            using var image = Image.Load(path);
            image.Mutate(x => x.Resize(200, 200));
            string filename = Path.GetFileName(path);
            string savePath = $"{Directory.GetCurrentDirectory()}/assets/scale_50/{filename}";
            image.Save(savePath); // TODO multiple scalings
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}