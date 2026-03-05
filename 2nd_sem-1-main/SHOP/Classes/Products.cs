using Avalonia.Platform;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using Dapper;
using Npgsql;

namespace SHOP.Classes
{
    public class Products 
    {
        //Review list for products
        public ObservableCollection<Review> ReviewList { get; set; } = new ObservableCollection<Review>();

        public Products()
        {
            
        }
        public Products(string s, string newproduct, string asus, string s1, double d, string labtop, string labtopJpg, string aLabtopForSchool)
        {
            throw new NotImplementedException();
        }
        public string Name { get; set; } = null!;
        public string DescriptionPath { get; set; } = null!;
        public string EAN_Number { get; set; } = null!;
        public int ID { get; set; }
        public double Price { get; set; }
        public string? FilePath { get; set; }
        public string? Type { get; set; }
        public string? Manufacturer { get; set; }

        public Bitmap ProductImage
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        var uri = new Uri(FilePath);
                        using var stream = AssetLoader.Open(uri);
                        return new Bitmap(stream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not load image: {FilePath} - {ex.Message}");
                }

                // Fallback if picture cant load
                var fallbackUri = new Uri("avares://SHOP/Assets/fallback.jpg");
                using var fallbackStream = AssetLoader.Open(fallbackUri);
                return new Bitmap(fallbackStream);
            }
        }
        
    }
    
}
