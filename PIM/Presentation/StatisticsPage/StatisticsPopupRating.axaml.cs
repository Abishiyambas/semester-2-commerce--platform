using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PIM_Logic;

namespace PIM.Presentation;

public partial class StatisticsPopupRating : Window
{
    private Product product;
    public StatisticsPopupRating(Product product)
    {
        InitializeComponent();
        this.product = product;
        LoadProductInfoIntoGrid();
        ProductNameLabel.Content = product.Name;
    }
    
    private void LoadProductInfoIntoGrid()
    {
        // Clear previous contents of the grid
        var RatingGrid = this.FindControl<Grid>("RatingGrid");
        RatingGrid.RowDefinitions.Clear();
        RatingGrid.Children.Clear();

        // Add product reviews to the grid
        foreach ( Review review in product.GetReviews () )
        {
            AddProductReviewToGrid(product, review);
        }
    }

    // This method adds the products ratings and comments to the grid
    private void AddProductReviewToGrid(Product product, Review review)
    {
        Control[] parameters =
        {
            new TextBlock { Text = review.Rating.ToString() },
            new TextBlock { Text = review.Comment }
        };

        var RatingGrid = this.FindControl<Grid>("RatingGrid");
        int rowIndex = RatingGrid.RowDefinitions.Count;
        RatingGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));

        for (int i = 0; i < parameters.Length; i++)
        {
            Control control = parameters[i];
            control.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
            RatingGrid.Children.Add(control);
            Grid.SetRow(control, rowIndex);
            Grid.SetColumn(control, i);
        }
    }
}