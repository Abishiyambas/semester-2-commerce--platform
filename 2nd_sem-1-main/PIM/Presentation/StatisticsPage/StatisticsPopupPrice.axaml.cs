using System;
using System.Linq;
using System.Runtime.Serialization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PIM_Logic;
using ScottPlot;
using ScottPlot.Avalonia;

namespace PIM.Presentation;

public partial class StatisticsPopupPrice : Window
{
    public StatisticsPopupPrice(Product product)
    {
        InitializeComponent();

        // Fetch price history and discounts
        var priceHistory = product.GetPriceHistroy ().OrderBy(ph => ph.Changed_at).ToArray();
        var discounts = product.GetDiscounts ().ToArray();
        AvaPlot avaPlot = this.Find<AvaPlot>("PriceHistory");

        if (priceHistory.Length == 0)
        {
            ProductNameLabel.Content = $"No Price Available for {product.Name}";
            avaPlot.IsVisible = false;
            return;
        }

        ProductNameLabel.Content = product.Name;

        // Prepare data for the scatterplot
        double[] dataX = priceHistory.Select(ph => ph.Changed_at.ToOADate()).ToArray(); // Dates as OLE Automation dates
        double[] dataY = priceHistory.Select(ph => (double)ph.New_price).ToArray();

        // Add scatterplot for price history
        avaPlot.Plot.Add.Scatter(dataX, dataY);

        foreach (var discount in discounts)
        {
            double startX = discount.Start_date.ToOADate();
            double endX = discount.End_date.ToOADate();

            Console.WriteLine($"Discount Period: {discount.Start_date} to {discount.End_date}");
            Console.WriteLine($"Discount Period (OLE Automation): {startX} to {endX}");

            // Filter the price history to only include points within the discount period
            var discountDataX = dataX.Where(x => x >= startX - 0.0001 && x <= endX + 0.0001).ToArray();

            Console.WriteLine($"Filtered DataX: {string.Join(", ", discountDataX)}");

            if (discountDataX.Length == 0)
            {
                Console.WriteLine("No data points found for this discount period. Attempting interpolation...");

                // Interpolation logic
                var beforeIndex = Array.FindLastIndex(dataX, x => x < startX);
                var afterIndex = Array.FindIndex(dataX, x => x > endX);

                if (beforeIndex >= 0 && afterIndex >= 0)
                {
                    double beforePrice = dataY[beforeIndex];
                    double afterPrice = dataY[afterIndex];
                    double interpolatedPrice = (beforePrice + afterPrice) / 2;

                    discountDataX = new[] { startX, endX };
                    var discountedPrices = new[] {
                        interpolatedPrice * (1 - discount.Discount_percentage / 100.0),
                        interpolatedPrice * (1 - discount.Discount_percentage / 100.0)
                    };

                    avaPlot.Plot.Add.Scatter(discountDataX, discountedPrices, color: Color.FromColor(System.Drawing.Color.Orange));
                }
                else
                {
                    Console.WriteLine("Unable to interpolate. Skipping this discount.");
                    continue;
                }
            }
            else
            {
                var startIndex = Array.IndexOf(dataX, discountDataX.First());
                var discountDataY = dataY.Skip(startIndex).Take(discountDataX.Length).ToArray();

                Console.WriteLine($"Filtered DataY: {string.Join(", ", discountDataY)}");

                if (discountDataY.Length > 0)
                {
                    var discountedPrices = discountDataY.Select(price => price * (1 - discount.Discount_percentage / 100.0)).ToArray();
                    Console.WriteLine($"Discounted Prices: {string.Join(", ", discountedPrices)}");

                    avaPlot.Plot.Add.Scatter(discountDataX, discountedPrices, color: Color.FromColor(System.Drawing.Color.Orange));
                }
            }
        }
        // Format the plot
        avaPlot.Plot.Axes.DateTimeTicksBottom();
        avaPlot.Plot.Axes.Right.MinimumSize = 50;
        avaPlot.Plot.Title($"Price History for {product.Name}");
        avaPlot.Plot.YLabel("Price");
        avaPlot.Plot.XLabel("Date");
        avaPlot.Refresh();
    }
}