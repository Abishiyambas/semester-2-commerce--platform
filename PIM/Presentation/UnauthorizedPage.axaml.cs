using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace PIM_Presentation
{
    public partial class UnauthorizedPage : UserControl
    {
        // Provide a callback so that the UnauthorizedPage can return back to the MainTabControl.
        public Action? ReturnToTabs { get; set; }

        public UnauthorizedPage()
        {
            InitializeComponent();
        }

        private void GoBack_Click(object? sender, RoutedEventArgs e)
        {
            // Call the callback when the button is clicked.
            ReturnToTabs?.Invoke();
        }
    }
}