using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using PIM_Logic;
using System;
using System.Threading.Tasks;

namespace PIM_Presentation;


public partial class SuppliersPage : Page
{
    private Supplier[] suppliersInGrid;
    public Supplier[] suppliersDisplayed { get { return suppliersInGrid; } }

    public SuppliersPage ( Pim pim )
    {
        InitializeComponent ();
        this.pim = pim;
        suppliersInGrid = Array.Empty<Supplier> ();
        LoadSuppliers ();
        RefreshButton.Content = ImageHandler ( "Refresh_icon" );
        ActiveEmployeeTextBlock.Text = EmployeeManager.ActiveEmployee != null ? $"Welcome {EmployeeManager.ActiveEmployee.Name}" : "";
    }

    public async void AddSupplierButtonHandler ( object sender, RoutedEventArgs e )
    {
        if ( sender is Button button )
        {
            var addsupplierWindow = new AddSupplierWindow( pim );
            var parentWindow = this.VisualRoot as Window;
            if ( parentWindow != null )
            {
                await addsupplierWindow.ShowDialog ( parentWindow );
            }
        }
    }

    private void SearchBox_KeyUp ( object? sender, Avalonia.Input.KeyEventArgs e )
    {
        if ( sender is TextBox searchBox )
        {
            LoadAllSuppliersIntoGrid ( searchBox.Text );
        }
    }

    public void RefreshButtonHandler ( object? sender, RoutedEventArgs args )
    {
        LoadSuppliers ();
    }

    public async Task LoadSuppliers ()
    {
        suppliersInGrid = await pim.GetAllSuppliers ();
        if ( suppliersInGrid.Length == 0 )
            return;
        LoadAllSuppliersIntoGrid ();
    }

    private void LoadAllSuppliersIntoGrid ( string searchParam = null )
    {
        SupplierGrid.RowDefinitions.Clear ();
        SupplierGrid.Children.Clear ();

        if ( searchParam == null )
        {
            foreach ( Supplier supplier in suppliersInGrid )
            {
                AddSupplierToGrid ( supplier );
            }
        }
        else
        {
            foreach ( Supplier supplier in suppliersInGrid )
            {
                if ( supplier.Name.ToLower ().Contains ( searchParam.ToLower () ) ||
                    supplier.Phone.ToLower ().Contains ( searchParam.ToLower () ) )

                {
                    AddSupplierToGrid ( supplier );
                }
            }
        }
    }

    private void AddSupplierToGrid ( Supplier supplier )
    {
        Control[] parameters =
        {
            SetControlIntoBorder(new TextBlock { Text = supplier.Name }),
            SetControlIntoBorder(new TextBlock { Text = supplier.Phone }),
            EditButton(supplier),
            DeleteButton(supplier)
        };

        int rowIndex = SupplierGrid.RowDefinitions.Count;
        SupplierGrid.RowDefinitions.Add ( new RowDefinition ( GridLength.Star ) );

        for ( int i = 0; i < parameters.Length; i++ )
        {
            try
            {
                Control control = parameters[i];
                control.VerticalAlignment = VerticalAlignment.Bottom;
                SupplierGrid.Children.Add ( control );
                Grid.SetRow ( control, rowIndex );
                Grid.SetColumn ( control, i );
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
        }
    }

    private Border EditButton ( Supplier supplier )
    {
        Button editButton = GetButton("edit-icon");

        editButton.Click += async ( s, e ) =>
        {
            var parentWindow = this.VisualRoot as Window;
            if ( parentWindow != null )
            {
                var editWindow = new EditSupplierWindow(pim, supplier);
                await editWindow.ShowDialog ( parentWindow );
                RefreshButtonHandler ( null, null );
            }
        };
        return SetControlIntoBorder ( editButton );
    }

    private Border DeleteButton ( Supplier supplier )
    {
        Button deleteButton = GetButton("remove-icon");

        deleteButton.Click += async ( s, e ) =>
        {
            var parentWindow = this.VisualRoot as Window;
            if ( parentWindow != null )
            {
                var removeWindow = new RemoveSupplierWindow(supplier);
                var result = await removeWindow.ShowDialog<bool>(parentWindow);
                if ( result )
                {
                    RefreshButtonHandler ( null, null );
                }
            }
        };
        return SetControlIntoBorder ( deleteButton );
    }
}
