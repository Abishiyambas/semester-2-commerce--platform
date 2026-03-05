using PIM_Logic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using System;
using System.Linq;
using System.Threading.Tasks;
using PIM.Presentation;
using ScottPlot.Avalonia;

namespace PIM_Presentation
{
    public partial class StatisticsPage : Page
    {
        private Product[] productsInGrid;

        public StatisticsPage(Pim pim)
        {
            InitializeComponent();
            this.pim = pim;
            productsInGrid = null;
            if (!AppEnvironment.IsTestEnvironment)
                ActiveEmployeeTextBlock.Text = EmployeeManager.ActiveEmployee.Name != null ? $"Welcome {EmployeeManager.ActiveEmployee.Name}" : "";
            RefreshButton.Content = ImageHandler ( "Refresh_icon" );
            LoadProducts ();
        }

        public async Task LoadProducts()
        {
            productsInGrid = await pim.GetAllProducts();
            LoadAllProductsIntoGrid();
            AvaPlot avaPlot = this.Find<AvaPlot>("Categories");
            LoadPieChart(avaPlot);
        }

        private async void LoadPieChart(AvaPlot avaPlot)
        {
            // Fetch products
            productsInGrid = await pim.GetAllProducts();

            // Group products by category and count them
            var categoryCounts = productsInGrid
                .GroupBy(p => p.GetCategory ()?.Name ?? "Uncategorized")
                .ToDictionary(g => g.Key, g => g.Count());

            // Prepare data for the pie chart
            double[] values = categoryCounts.Values.Select(count => (double)count).ToArray();
            string[] labels = categoryCounts.Keys.ToArray();
            
            avaPlot.Plot.Clear();
            var pie = avaPlot.Plot.Add.Pie(values);

            // Set labels to show category name and product count
            for (int i = 0; i < pie.Slices.Count; i++)
            {
                pie.Slices[i].Label = $"{labels[i]}: {categoryCounts[labels[i]]}";
                pie.Slices[i].LabelFontSize = 20; // Optional: Adjust font size
            }

            // Remove the background grid
            avaPlot.Plot.Axes.Frameless();
            avaPlot.Plot.HideGrid();

            // Adjust zoom level to make the pie chart appear closer
            avaPlot.Plot.Axes.SetLimits(-1.5, 1.5, -1.5, 1.5);
            avaPlot.Refresh();
        }

        
        public async void RefreshButtonHandler(object? sender, RoutedEventArgs args)
        {
            LoadProducts();
        }

        private void SearchBox_KeyUp(object? sender, KeyEventArgs e)
        {
            if (sender is TextBox SearchBox)
            {
                LoadAllProductsIntoGrid( SearchBox.Text );
            }
        }

        // This function will populate the grid with the fetched product data
        private void LoadAllProductsIntoGrid(string searchParam = null)
        {
            var productGrid = this.FindControl<Grid>("ProductGrid");
            productGrid.RowDefinitions.Clear();
            productGrid.Children.Clear();

            var productsToDisplay = string.IsNullOrWhiteSpace(searchParam)
                ? productsInGrid
                : FilterProducts(productsInGrid, searchParam);

            foreach (var product in productsToDisplay)
            {
                AddProductToGrid(product);
            }
        }
        
        public Product[] FilterProducts ( Product[] products, string searchParam)
        {
            if ( products == null) return Array.Empty<Product>();

            return products.Where(product =>
                product.Name.Contains(searchParam, StringComparison.OrdinalIgnoreCase) ||
                product.GetCategory ().Name.Contains(searchParam, StringComparison.OrdinalIgnoreCase) ||
                product.GetSupplier ().Name.Contains(searchParam, StringComparison.OrdinalIgnoreCase) ||
                product.GetSku ().Contains(searchParam, StringComparison.OrdinalIgnoreCase) ||
                product.Description.Contains(searchParam, StringComparison.OrdinalIgnoreCase)
            ).ToArray();
        }

        // This function adds a product to the grid
        private void AddProductToGrid(Product product)
        {
            Control[] parameters =
            {
                new TextBlock { Text = product.Name },
                CreateClickablePriceTextBlock(product),
                CreateClickableRatingTextBlock(product),
                new TextBlock { Text = product.Status ? "✅" : "⛔" }
            };

            var productGrid = this.FindControl<Grid>("ProductGrid");
            int rowIndex = productGrid.RowDefinitions.Count;
            productGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));

            for (int i = 0; i < parameters.Length; i++)
            {
                Control control = parameters[i];
                control.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                productGrid.Children.Add(control);
                Grid.SetRow(control, rowIndex);
                Grid.SetColumn(control, i);
            }
        }

        // Creates the TextBlock for the price that is clickable to open the price popup
        private Control CreateClickablePriceTextBlock(Product product)
        {
            var priceTextBlock = new TextBlock
            {
                Text = product.Price.ToString(),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };

            ToolTip.SetTip(priceTextBlock, "Click to view price history");
            
            priceTextBlock.PointerPressed += async (sender, e) =>
            {
                var pricePopup = new StatisticsPopupPrice(product);
                var parentWindow = this.VisualRoot as Window;
                if (parentWindow != null)
                {
                    await pricePopup.ShowDialog(parentWindow);
                }
            };

            return priceTextBlock;
        }

        // Creates the TextBlock for the rating that is clickable to open the rating popup
        private Control CreateClickableRatingTextBlock(Product product)
        {
            var ratingTextBlock = new TextBlock
            {
                Text = ((int)product.GetAverageReviewRating()).ToString(),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            
            ToolTip.SetTip(ratingTextBlock, "Click to view customer rating breakdown");

            ratingTextBlock.PointerPressed += async (sender, e) =>
            {
                var ratingPopup = new StatisticsPopupRating(product);
                var parentWindow = this.VisualRoot as Window;
                if (parentWindow != null)
                {
                    await ratingPopup.ShowDialog(parentWindow);
                }
            };

            return ratingTextBlock;
        }
    }
}