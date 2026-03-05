using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using PIM_Logic;

namespace PIM_Presentation;

public partial class CreateCategoryWindow : Window
{
    private readonly Pim _pim;
    private readonly Func<Task> _onSuccess;

    private void Reset()
    {
        CategoryNameBox.Text = string.Empty;
    }
    public CreateCategoryWindow ( Pim pim, Func<Task> onSuccess )
    {
        InitializeComponent ();
        _pim = pim;
        _onSuccess = onSuccess;

        CreateButton.Click += async ( _, _ ) =>
        {
            var name = CategoryNameBox.Text.Trim();
            if ( !string.IsNullOrEmpty ( name ) )
            {
                await _pim.InsertCategory ( new Category ( 0, name ) );
                await _onSuccess.Invoke ();
                Reset ();
            }
        };

        CancelButton.Click += ( _, _ ) => Close ();
    }
}
