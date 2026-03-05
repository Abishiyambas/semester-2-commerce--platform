using Avalonia.Controls;
using PIM_Logic;
using System;
using System.Threading.Tasks;

namespace PIM_Presentation;

public partial class UpdateCategoryWindow : Window
{
    private readonly Pim _pim;
    private readonly Category _category;
    private readonly Func<Task> _onUpdated;

    public UpdateCategoryWindow ( Pim pim, Category category, Func<Task> onUpdated )
    {
        InitializeComponent ();
        _pim = pim;
        _category = category;
        _onUpdated = onUpdated;

        CategoryNameBox.Text = category.Name;

        UpdateButton.Click += async ( _, _ ) =>
        {
            var newName = CategoryNameBox.Text?.Trim();
            if ( !string.IsNullOrEmpty ( newName ) )
            {
                _category.Name = newName;
                await _pim.UpdateCategory ( _category );
                await _onUpdated ();
                Close ();
            }
        };

        CancelButton.Click += ( _, _ ) => Close ();
    }
}