using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using PIM_Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIM_Presentation;

public partial class ProductsPage : Page
{
    private readonly Pim pim;
    private Product[] productsInGrid;
    private string? currentSearchParam;

    public Product[] ProductsDisplayed { get { return productsInGrid; } }

    public ProductsPage ( Pim pim )
    {
        InitializeComponent ();
        this.pim = pim;
        productsInGrid = Array.Empty<Product> ();

        if ( !AppEnvironment.IsTestEnvironment )
            ActiveEmployeeTextBlock.Text = EmployeeManager.ActiveEmployee != null ? $"Welcome {EmployeeManager.ActiveEmployee.Name}" : "";

        LoadProducts ();
        RefreshButton.Content = ImageHandler ( "Refresh_icon" );
    }

    public async void AddProductButtonHandler ( object? sender, RoutedEventArgs args )
    {
        if ( sender is Button button )
        {
            var addProductWindow = new AddProductWindow(pim, LoadProducts );
            var parentWindow = this.VisualRoot as Window;
            if ( parentWindow != null )
            {
                await addProductWindow.ShowDialog ( parentWindow );
            }
        }
    }

    private void SearchBox_KeyUp ( object? sender, Avalonia.Input.KeyEventArgs e )
    {
        if ( sender is TextBox searchBox )
        {
            currentSearchParam = searchBox.Text;
            LoadAllProductsIntoGrid ( searchBox.Text );
        }
    }

    public void RefreshButtonHandler ( object? sender, RoutedEventArgs args )
    {
        LoadProducts ();
    }

    public async Task LoadProducts ()
    {
        productsInGrid = await pim.GetAllProducts ();
        LoadAllProductsIntoGrid ( currentSearchParam );
    }

    private void LoadAllProductsIntoGrid ( string? searchParam = null )
    {
        var productGrid = this.FindControl<Grid>("ProductGrid");

        if ( productGrid is not null )
        {
            productGrid.RowDefinitions.Clear ();
            productGrid.Children.Clear ();

            var productsToDisplay = string.IsNullOrWhiteSpace(searchParam)
                ? productsInGrid                // if there is no search parameter
                : FilterProducts(searchParam);  // if there is a search parameter

            var sortedProducts = ApplySorting(productsToDisplay, ProductFilterComboBox.SelectedIndex);

            foreach ( var product in sortedProducts )
            {
                AddProductToGrid ( product );
            }
        }
    }

    public IEnumerable<Product> ApplySorting ( IEnumerable<Product> products, int selectedIndex )
    {
        return selectedIndex switch
        {
            0 => products.OrderBy ( product => product.Name ), // Sort by Name (Ascending)
            1 => products.OrderByDescending ( product => product.Name ), // Sort by Name (Descending)
            2 => products.OrderByDescending ( product => product.GetAverageReviewRating () ), // Sort by Rating (Descending)
            3 => products.OrderBy ( product => product.GetAverageReviewRating () ), // Sort by Rating (Ascending)
            4 => products.OrderByDescending ( product => product.GetEffectivePrice () ), // Sort by Price (Descending)
            5 => products.OrderBy ( product => product.GetEffectivePrice () ), // Sort by Price (Ascending)
            _ => products // Default: No sorting
        };
    }

    public Product[] FilterProducts ( string searchParam )
    {
        if ( productsInGrid == null )
            return Array.Empty<Product> ();

        return productsInGrid.Where ( product =>
            product.Name.Contains ( searchParam, StringComparison.OrdinalIgnoreCase ) ||
            product.GetCategory ().Name.Contains ( searchParam, StringComparison.OrdinalIgnoreCase ) ||
            product.GetSupplier ().Name.Contains ( searchParam, StringComparison.OrdinalIgnoreCase ) ||
            product.GetSku ().Contains ( searchParam, StringComparison.OrdinalIgnoreCase ) ||
            product.Description.Contains ( searchParam, StringComparison.OrdinalIgnoreCase )
        ).ToArray ();
    }

    private void AddProductToGrid ( Product product )
    {
        Control[] parameters =
        {
            SetControlIntoBorder ( new TextBlock { Text = product.Name } ),
            SetControlIntoBorder ( new TextBlock { Text = product.Price == product.GetEffectivePrice() ? $"{product.Price}" : $"{product.Price} // {product.GetEffectivePrice()}"}),
            SetControlIntoBorder ( new TextBlock { Text = product.GetCategory () == null ? "" : product.GetCategory ().Name } ),
            SetControlIntoBorder ( new TextBlock { Text = product.GetSupplier ().Name } ),
            SetControlIntoBorder ( new TextBlock { Text = ( (int) product.GetAverageReviewRating()).ToString() } ),
            SetControlIntoBorder ( new TextBlock { Text = product.Status ? "✅" : "⛔" } ),
            StockButton ( product ),
            EditButton( product ),
            DeleteButton ( product ),
            DetailsButton(product)
        };

        int rowIndex = ProductGrid.RowDefinitions.Count;
        ProductGrid.RowDefinitions.Add ( new RowDefinition ( GridLength.Star ) ); // add new grid row with size auto

        for ( int i = 0; i < parameters.Length; i++ )
        {
            Control control = parameters[i];
            control.VerticalAlignment = VerticalAlignment.Bottom;
            ProductGrid.Children.Add ( control );
            Grid.SetRow ( control, rowIndex );
            Grid.SetColumn ( control, i );
        }
    }

    private Border StockButton ( Product product )
    {
        Grid stockGrid = new Grid();
        stockGrid.ColumnDefinitions.AddRange ( [new ColumnDefinition ( new GridLength ( 35 ) ), new ColumnDefinition ( GridLength.Star )] );

        var stockColor = Brushes.Green;
        if ( product.Current_stock == 0 )
            stockColor = Brushes.Red;
        else if ( product.Current_stock < product.Max_stock * 0.2 )
            stockColor = Brushes.Orange;

        TextBlock stockText = new TextBlock
        {
            Text = product.Current_stock.ToString(),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = stockColor
        };

        Button stockButton = GetButton("plus-icon");
        ToolTip.SetTip ( stockButton, "Order stock for this product" );

        stockButton.Click += async ( sender, e ) =>
        {
            var popup = new StockOrderPopup(pim, product);

            popup.StockUpdated += ( orderedAmount ) =>
            {
                int newStock = product.Current_stock + orderedAmount;
                product.Current_stock = newStock;
                stockText.Text = newStock.ToString ();
                Console.WriteLine ( $"Ordered: {orderedAmount}" );
            };

            popup.OrderCanceled += () =>
            {
                Console.WriteLine ( "User canceled." );
            };

            var parentWindow = this.VisualRoot as Window;
            if ( parentWindow != null )
            {
                await popup.ShowDialog ( parentWindow );
                LoadProducts ();
            }
        };

        stockGrid.Children.Add ( stockText );
        stockGrid.Children.Add ( stockButton );
        Grid.SetColumn ( stockText, 0 );
        Grid.SetColumn ( stockButton, 1 );

        return SetControlIntoBorder ( stockGrid );
    }

    private Border EditButton ( Product product )
    {
        Button editButton = GetButton( "edit-icon" );
        ToolTip.SetTip ( editButton, "Edit this product" );

        editButton.Click += async ( s, e ) =>
        {
            var parentWindow = this.VisualRoot as Window;
            if ( parentWindow != null )
            {
                var editWindow = new EditProductWindow(pim, product);
                await editWindow.ShowDialog ( parentWindow );
                LoadProducts ();
            }
        };

        return SetControlIntoBorder ( editButton );
    }

    private Border DeleteButton ( Product product )
    {
        Button deleteButton = GetButton("remove-icon");
        ToolTip.SetTip ( deleteButton, "Delete this product" );

        deleteButton.Click += async ( s, e ) =>
        {
            var parentWindow = this.VisualRoot as Window;
            if ( parentWindow != null )
            {
                var removeWindow = new RemoveItemWindow(product);
                var result = await removeWindow.ShowDialog<bool>(parentWindow);
                if ( result )
                {
                    await pim.DeleteProduct ( product.Product_id );

                    LoadProducts ();
                }
            }
        };
        return SetControlIntoBorder ( deleteButton );
    }

    private Border DetailsButton ( Product product )
    {
        Button detailsButton = GetButton("information_icon");
        ToolTip.SetTip ( detailsButton, "Show product details" );

        detailsButton.Click += async ( s, e ) =>
        {
            var parentWindow = this.VisualRoot as Window;
            if ( parentWindow != null )
            {
                var detailsWindow = new ProductDetailsWindow(product); // Du skal oprette denne window
                await detailsWindow.ShowDialog ( parentWindow );
            }
        };

        return SetControlIntoBorder ( detailsButton );
    }
}