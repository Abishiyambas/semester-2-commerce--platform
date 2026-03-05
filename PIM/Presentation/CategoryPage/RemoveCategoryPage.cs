using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PIM_Logic;

namespace PIM_Presentation
{
    public partial class RemoveCategoryPage : Window
    {
        private readonly Pim _pim;
        private readonly Category _category;
        private readonly Action _onDeleteComplete;

        public RemoveCategoryPage(Pim pim, Category category, Action onDeleteComplete)
        {
            InitializeComponent();
            _pim = pim;
            _category = category;
            _onDeleteComplete = onDeleteComplete;

            CategoryRow.Text = _category.Name;
        }

        private void Cancel_Click(object? sender, RoutedEventArgs e)
        {
            Close(false);
        }

        private async void Delete_Click(object? sender, RoutedEventArgs e)
        {
            await _pim.DeleteCategory(_category.Category_id);
            _onDeleteComplete.Invoke();
            Close(true);
        }
    }
}