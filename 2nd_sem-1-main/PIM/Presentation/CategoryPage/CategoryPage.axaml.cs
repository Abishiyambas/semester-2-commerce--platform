using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Layout;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PIM_Logic;
using Avalonia;

namespace PIM_Presentation;

public partial class CategoriesPage : Page
{
    private List<Category> _categories;
    private string _currentSearch = string.Empty;

    public List<Category> CategoriesDisplayed { get { return _categories; } }

    public CategoriesPage ( Pim pim )
    {
        InitializeComponent ();
        this.pim = pim;

        RefreshButton.Content = ImageHandler ( "Refresh_icon" );
        ActiveEmployeeTextBlock.Text = EmployeeManager.ActiveEmployee != null ? $"Welcome {EmployeeManager.ActiveEmployee.Name}" : "";

        LoadCategories ();
    }

    public async Task LoadCategories ()
    {
        var allProducts = (await pim.GetAllProducts()).ToList();
        _categories = ( await pim.GetAllCategories () ).ToList ();

        foreach ( var cat in _categories )
        {
            cat.SetProductCount ( allProducts.Count ( p => p.Category_id == cat.Category_id ) );
        }

        if ( _categories.Count == 0 )
            return;
        LoadCategoriesIntoStack ();
    }

    private void LoadCategoriesIntoStack ()
    {
        CategoryStack.Children.Clear ();

        IEnumerable<Category> filteredCategories = _categories;

        if ( !string.IsNullOrWhiteSpace ( _currentSearch ) )
        {
            filteredCategories = _categories
                .Where ( cat => cat.Name.Contains ( _currentSearch, StringComparison.OrdinalIgnoreCase ) );
        }

        foreach ( var cat in filteredCategories )
        {
            Grid row = new()
            {
                ColumnDefinitions = new ColumnDefinitions("7*,5*,2*,2*"),
                VerticalAlignment = VerticalAlignment.Center
            };

            Control[] parameters =
            {
                SetControlIntoBorder(CreateTextBlock ( cat.Name )),
                SetControlIntoBorder(CreateTextBlock ( cat.GetProductCount ().ToString () )),
                SetControlIntoBorder (CreateButton ( "edit-icon", EditCategoryHandler, cat, "Edit this category" )),
                SetControlIntoBorder(CreateButton ( "remove-icon", DeleteCategoryHandler, cat, "Delete this category" ))
            };

            for ( int i = 0; i < parameters.Length; i++ )
            {
                Grid.SetColumn ( parameters[i], i );
                row.Children.Add ( parameters[i] );
            }
            
            CategoryStack.Children.Add ( row );
        }
    }

    private TextBlock CreateTextBlock ( string text )
    {
        TextBlock tb = new() { Text = text, VerticalAlignment = VerticalAlignment.Center };
        return tb;
    }

    private Button CreateButton ( string iconName, EventHandler<RoutedEventArgs> clickHandler, object dataContext, string tooltip )
    {
        Button btn = new()
        {
            Content = ImageHandler(iconName),
            Background = Brushes.Transparent,
            Padding = new Thickness ( 0 ),
            Margin = new Thickness ( 0 ),
            Width = 25,
            Height = 25,
            DataContext = dataContext
        };

        btn.Click += clickHandler;
        
        ToolTip.SetTip(btn, tooltip);
        
        return btn;
    }

    private async void AddCategoryHandler ( object? sender, RoutedEventArgs e )
    {
        var window = new CreateCategoryWindow(pim, LoadCategories);
        await window.ShowDialog ( (Window) this.VisualRoot );
    }

    private async void EditCategoryHandler ( object? sender, RoutedEventArgs e )
    {
        if ( ( sender as Button )?.DataContext is Category category )
        {
            var window = new UpdateCategoryWindow(pim, category, LoadCategories);
            await window.ShowDialog ( (Window) this.VisualRoot );
        }
    }

    private async void DeleteCategoryHandler(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is Category category)
        {
            var window = new RemoveCategoryPage(pim, category, () => LoadCategories());
            await window.ShowDialog((Window)this.VisualRoot);
        }
    }


    private void RefreshButtonHandler ( object sender, RoutedEventArgs e )
    {
        LoadCategories ();
    }

    private void SearchBox_KeyUp ( object? sender, Avalonia.Input.KeyEventArgs e )
    {
        if ( sender is TextBox tb )
        {
            _currentSearch = tb.Text ?? "";
            LoadCategoriesIntoStack ();
        }
    }

}
