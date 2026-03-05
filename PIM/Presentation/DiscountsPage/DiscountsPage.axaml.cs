using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Npgsql;
using PIM_Logic;
using PIM_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;

namespace PIM_Presentation;

public partial class DiscountsPage : Page
{
    private List<Product> products = new();
    private List<Category> categories = new();

    private int? singleProductId;
    private int? groupingCategoryId;

    public DiscountsPage(Pim pim)
    {
        this.pim = pim;
        InitializeComponent ();
        
        this.Loaded += async (_, __) => await LoadAllDataAsync();
    }

    private async Task LoadAllDataAsync()
    {
        products   = (await pim.GetAllProducts()).ToList();
        categories = (await pim.GetAllCategories()).ToList();
        
        foreach ( var cat in categories )
        {
            cat.SetProductCount ( products.Count ( p => p.Category_id == cat.Category_id ) );
        }

        await PopulateProductDiscountGrids();
        await PopulateCategoryDiscountGrids();
    }
    
    // ——— Single-discount form ———
    private void OnSearchTextChanged(object? s, RoutedEventArgs e)
    {
        string term = SearchBox.Text?.Trim() ?? "";
        List<Product> matches = products
            .Where(p => p.Name.Contains(term, StringComparison.OrdinalIgnoreCase)
                        || p.GetSku () .Contains(term, StringComparison.OrdinalIgnoreCase))
            .ToList();

        SuggestionsList.ItemsSource = matches;
        SuggestionsPopup.IsOpen = matches.Any();
    }

    private void OnSuggestionChosen(object? s, SelectionChangedEventArgs e)
    {
        if (SuggestionsList.SelectedItem is Product p)
        {
            singleProductId     = p.Product_id;
            NameBox.Text        = p.Name;
            SkuBox.Text         = p.GetSku ();
            CatBox.Text         = p.GetCategory()?.Name ?? "";
            NormalPriceBox.Text = p.Price.ToString("0.00");
        }
        SuggestionsPopup.IsOpen = false;
    }

    private void ClearSingleFormHandler(object? sender, RoutedEventArgs e)
    {
        ClearSingleForm();
    }

    private void ClearSingleForm()
    {
        singleProductId = null;
        //SearchBox.Text = "";
        NameBox.Text = SkuBox.Text = CatBox.Text = NormalPriceBox.Text =
            DiscPercentBox.Text = DiscPriceBox.Text = "";
        StartDatePicker.SelectedDate = EndDatePicker.SelectedDate = null;
    }

    private async void OnConfirmSingle(object? sender, RoutedEventArgs e)
    {
        if (singleProductId == null) return;

        // 1) parse the normal price
        if (!decimal.TryParse(NormalPriceBox.Text, out var normalPrice))
            return;

        // 2) figure newPrice and pct
        int? pct = null;
        decimal? newPrice = null;
        if (int.TryParse(DiscPercentBox.Text, out var x))
        {
            pct = x;
            newPrice = normalPrice * (100 - x) / 100m;
        }
        else if (decimal.TryParse(DiscPriceBox.Text, out var y))
        {
            newPrice = y;
            pct = (int)Math.Round((1 - (y / normalPrice)) * 100);
        }
        if (pct == null || newPrice == null) return;

        // 3) pull and UTC-normalize your dates
        var localStart = StartDatePicker.SelectedDate
            ?? throw new InvalidOperationException("Start date required");
        var localEnd = EndDatePicker.SelectedDate
            ?? throw new InvalidOperationException("End date required");
        var utcStart = localStart.ToUniversalTime();
        var utcEnd   = localEnd  .ToUniversalTime();

        Discount discount = new Discount
        (
            pct.Value,
            utcStart.Date,
            utcEnd.Date,
            singleProductId,
            null
        );
        
        // 5) insert into price_history (use UTC now)
        var utcNow = DateTime.UtcNow;

        await pim.InsertPriceHistory ( new PriceHistory ( newPrice.Value, normalPrice, singleProductId.Value ) );

        //await DatabaseUtility.ExecuteNonQueryAsync(
        //    @"INSERT INTO price_history(new_price, old_price, product_id, changed_at)
        //      VALUES (@n, @o, @pid, @c)",
        //    new NpgsqlParameter("@n", newPrice.Value),
        //    new NpgsqlParameter("@o", normalPrice),
        //    new NpgsqlParameter("@pid", singleProductId.Value),
        //    new NpgsqlParameter("@c", NpgsqlDbType.TimestampTz) { Value = utcNow }
        //);

       await pim.InsertDiscount(discount);
        
        ClearSingleForm();
    }
    // ——— Category-discount form ———
    private void OnCategorySearchTextChanged(object? s, RoutedEventArgs e)
    {
        string term = CategorySearchBox.Text?.Trim() ?? "";
        List<Category> matches = categories
            .Where(c => c.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
            .ToList();

        CategorySuggestionsList.ItemsSource = matches;
        CategorySuggestionsPopup.IsOpen = matches.Any();
    }

    private void OnCategorySuggestionChosen(object? s, SelectionChangedEventArgs e)
    {
        if (CategorySuggestionsList.SelectedItem is Category c)
        {
            groupingCategoryId = c.Category_id;
            CategoryNameBox.Text = c.Name;
            ProductCountBox.Text = c.GetProductCount().ToString();
        }
        CategorySuggestionsPopup.IsOpen = false;
    }

    private void ClearGroupingFormHandler(object? sender, RoutedEventArgs e)
    {
        ClearGroupingForm();
    }

    private void ClearGroupingForm()
    {
        groupingCategoryId = null;
        CategorySearchBox.Text = 
            CategoryNameBox.Text =
                ProductCountBox.Text =
                    GroupingDiscPercentBox.Text = "";
        GroupingStartDatePicker.SelectedDate =
            GroupingEndDatePicker.SelectedDate = null;
    }

    private void OnConfirmGrouping(object? sender, RoutedEventArgs e)
    {
        if (groupingCategoryId == null) return;
        if (!int.TryParse(GroupingDiscPercentBox.Text, out var pct)) return;
     
        var localStart = GroupingStartDatePicker.SelectedDate
                         ?? throw new InvalidOperationException("Start date required");
        var localEnd = GroupingEndDatePicker.SelectedDate
                       ?? throw new InvalidOperationException("End date required");
        var utcStart = localStart.ToUniversalTime();
        var utcEnd   = localEnd  .ToUniversalTime();

        var batch = products.Where(p => p.Category_id == groupingCategoryId.Value);
        foreach (var p in batch)
        {
            Discount discount = new Discount
            (
                pct,
                utcStart.Date,
                utcEnd.Date,
                null,
                groupingCategoryId
            );
        }

        ClearGroupingForm();
    }

    // ======== SHOWING IN GRID =========

    private void PopulateDiscountGrid(Grid target, IEnumerable<(DisplayDiscount view, Discount model)> items)
    {
        if ( target is null )
            return;

        target.RowDefinitions.Clear();
        target.Children.Clear();

        foreach (var (d, model) in items)
        {
            int rowIndex = target.RowDefinitions.Count;
            target.RowDefinitions.Add(new RowDefinition(GridLength.Star));

            var controls = new Control[]
            {
                SetControlIntoBorder(new TextBlock { Text = d.Name }),
                SetControlIntoBorder(new TextBlock { Text = d.StartDate.ToString("d") }),
                SetControlIntoBorder(new TextBlock { Text = d.EndDate  .ToString("d") }),
                SetControlIntoBorder(new TextBlock { Text = $"{d.DiscountPercent} %" }),
                SetControlIntoBorder(EditButton(d, model)),
                SetControlIntoBorder(DeleteButton(d, model))
            };

            for (int col = 0; col < controls.Length; col++)
            {
                target.Children.Add(controls[col]);
                Grid.SetRow(controls[col], rowIndex);
                Grid.SetColumn(controls[col], col);
            }
        }
    }


    private async Task PopulateProductDiscountGrids()
    {
        var now = DateTime.Now;
        var all = new List<(DisplayDiscount, Discount)>();

        foreach (var p in products)
        {
            var discounts = await pim.GetDiscounts(p.Product_id);
            foreach (var d in discounts)
            {
                all.Add((
                    new DisplayDiscount {
                        Name            = p.Name,
                        DiscountPercent = d.Discount_percentage,
                        StartDate       = d.Start_date,
                        EndDate         = d.End_date
                    },
                    d
                ));
            }
        }

        PopulateDiscountGrid(PastGrid,    all.Where(x => x.Item1.EndDate   < now));
        PopulateDiscountGrid(ActiveGrid,  all.Where(x => x.Item1.StartDate <= now && x.Item1.EndDate >= now));
        PopulateDiscountGrid(UpcomingGrid,all.Where(x => x.Item1.StartDate  > now));
    }

    
    private async Task PopulateCategoryDiscountGrids()
    {
        var now = DateTime.Now;
        // get all category‐level discounts
        var catDiscounts = await pim.GetAllCategoryDiscounts();

        // build the tuple list
        var all = catDiscounts
            .Select(d =>
            {
                var cat = categories.FirstOrDefault(c => c.Category_id == d.Category_id);
                return (
                    new DisplayDiscount {
                        Name            = cat?.Name ?? "(unknown)",
                        DiscountPercent = d.Discount_percentage,
                        StartDate       = d.Start_date,
                        EndDate         = d.End_date
                    },
                    d
                );
            })
            .ToList();

        PopulateDiscountGrid(PastCatGrid,    all.Where(x => x.Item1.EndDate   < now));
        PopulateDiscountGrid(ActiveCatGrid,  all.Where(x => x.Item1.StartDate <= now && x.Item1.EndDate >= now));
        PopulateDiscountGrid(UpcomingCatGrid,all.Where(x => x.Item1.StartDate  > now));
    }

    // ======== Resources ======== 
    
    private Border EditButton(DisplayDiscount d, Discount model)
    {
        var editButton = new Button
        {
            Padding = new Thickness(0),
            Background = Brushes.Transparent,
            Width = 25,
            Height = 25
        };
        editButton.Content = ImageHandler("edit-icon");
        ToolTip.SetTip(editButton, "Edit this discount");
        editButton.Click += async (_, __) =>
        {
            var owner = this.VisualRoot as Window;
            if (owner == null) return;

            // pass in the model and the display name
            var dlg = new EditDiscountWindow(pim, model, d.Name);
            bool? ok = await dlg.ShowDialog<bool?>(owner);
            if (ok == true)
            {
                // whichever tab is active, repopulate
                if (MainTab.SelectedIndex == 0)
                    await PopulateProductDiscountGrids();
                else
                    await PopulateCategoryDiscountGrids();
            }
        };
        return SetControlIntoBorder(editButton);
    }

    private Border DeleteButton(DisplayDiscount d, Discount model)
    {
        var deleteButton = new Button
        {
            Padding = new Thickness(0),
            Background = Brushes.Transparent,
            Width = 25,
            Height = 25
        };
        deleteButton.Content = ImageHandler("Remove-icon");
        ToolTip.SetTip(deleteButton, "Delete this discount");
        deleteButton.Click += async (_, __) =>
        {
            var owner = this.VisualRoot as Window;
            if (owner == null) return;

            var dlg = new DeleteDiscountWindow(pim, model, d.Name);
            bool? deleted = await dlg.ShowDialog<bool?>(owner);
            if (deleted == true)
            {
                if (MainTab.SelectedIndex == 0)
                    await PopulateProductDiscountGrids();
                else
                    await PopulateCategoryDiscountGrids();
            }
        };
        return SetControlIntoBorder(deleteButton);
    }

    private async void OnMainTabSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // 1) Was it MainTab that raised this?
        if (sender != MainTab)
            return;

        // 2) Now it's safe to use SelectedIndex
        switch (MainTab.SelectedIndex)
        {
            // Product Discounts tab
            case 0:
                await PopulateProductDiscountGrids();
                break;

            // Category Discounts tab
            case 1:
                // first-time default selection
                if (groupingCategoryId == null && categories.Count > 0)
                {
                    var firstCat = categories[0];
                    groupingCategoryId   = firstCat.Category_id;
                    CategoryNameBox.Text = firstCat.Name;
                    ProductCountBox.Text = firstCat.GetProductCount().ToString();
                }
                await PopulateCategoryDiscountGrids();
                break;
        }
    }

    private async void OnProductTabSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        PopulateProductDiscountGrids();
    }
    
    private async void OnCategoryTabSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        PopulateCategoryDiscountGrids();
    }
}

public class DisplayDiscount
{
    public string   Name            { get; init; }
    public DateTime StartDate       { get; init; }
    public DateTime EndDate         { get; init; }
    public int      DiscountPercent { get; init; }
}