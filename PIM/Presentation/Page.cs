using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using PIM_Logic;
using Avalonia.Media;
using Avalonia;
using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Layout;

namespace PIM_Presentation
{
    public class Page : UserControl
    {
        protected Pim pim;

        protected static Image ImageHandler ( string imageName )
        {
            if ( AppEnvironment.IsTestEnvironment )
                return new Image ();
            

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
            var imagePath = Path.Combine(projectRoot, "Content", imageName + ".png");

            Image image = new Image()
            {
                Source = new Bitmap(imagePath),
                Stretch = Stretch.UniformToFill,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            return image;
        }

        protected static Border SetControlIntoBorder ( Control control )
        {
            Border border = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 0, 0, 1),
                Child = control
            };
            return border;
        }

        protected Button GetButton(string imageName)
        {
            return new Button
            {
                Content = ImageHandler ( imageName ),
                Padding = new Thickness ( 0 ),
                Background = Brushes.Transparent,
                //HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness ( 0 ),
                Width = 25,
                Height = 25
            };
        }

    }
}
