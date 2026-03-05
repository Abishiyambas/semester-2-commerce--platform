using System;
using System.Linq;

namespace PIM_Logic;

public class Product
{
    private int productId, maxStock, currentStock;
    private string name, description, sku;
    private bool status;
    private decimal price;
    

    private DateTime createdAt, updatedAt;

    private int supplierId;
    private int? categoryId;

    private Review[] reviews;
    private Supplier supplier;
    private Category category;
    private PriceHistory[] priceHistroy;
    private Discount[] discount;

    // 
    // All properties WILL be inserted into database, excepted the first "id"
    //
    public int Product_id { get { return productId; } set { productId = value; } }
    public string Name { get { return name; } set { name = value; } }
    public string Description { get { return description; } set { description = value; } }
    public bool Status { get { return status; } set { status = value; } }
    public decimal Price
    {
        get { return price; }
        set
        {   // if the current price and the new price does not match a new price history is created and saved
            if ( value != price )
            {
                var priceHistory = new PriceHistory(value, price, productId);
                //Pim.SavePriceHistory ( priceHistory );

                price = value;
            }
        }
    }
    public int Max_stock { get { return maxStock; } set { maxStock = value; } }
    public int Current_stock { get { return currentStock; } set { currentStock = value; } }
    public DateTime Created_at { get { return createdAt; } }
    public DateTime Updated_at { get { return updatedAt; } set { updatedAt = value; } }
    public int? Category_id { get { return categoryId; } set { categoryId = value; } }
    public int Supplier_id { get { return supplierId; } set { supplierId = value; } }

    public Product ( Npgsql.NpgsqlDataReader reader )
    {
        this.productId = reader.GetInt32 ( 0 );
        this.name = reader.GetString ( 1 );
        this.description = reader.IsDBNull ( 2 ) ? "Has no description" : reader.GetString ( 2 );
        this.sku = reader.GetString ( 3 );
        this.status = reader.GetBoolean ( 4 );
        this.price = reader.GetDecimal ( 5 );
        this.maxStock = reader.GetInt32 ( 6 );
        this.currentStock = reader.GetInt32 ( 7 );
        this.createdAt = reader.GetDateTime ( reader.GetOrdinal ( "created_at" ) );
        this.updatedAt = reader.GetDateTime ( reader.GetOrdinal ( "updated_at" ) );
        this.categoryId = reader.IsDBNull ( 10 ) ? null : reader.GetInt32 ( 10 );
        this.supplierId = reader.GetInt32 ( 11 );
    }


    // Create new product
    public Product ( string name, string description, bool status,
        decimal price, int maxStock, int currentStock, int? categoryId, int supplierId )
    {
        this.name = name;
        this.description = description;
        this.status = status;
        this.price = price;
        this.maxStock = maxStock;
        this.currentStock = currentStock;
        this.categoryId = categoryId;
        this.supplierId = supplierId;
        this.createdAt = DateTime.Now;
        this.updatedAt = DateTime.Now;
    }

    public string GetSku ()
    { return sku; }

    public void SetSku ( string value )
    {
        if ( AppEnvironment.IsTestEnvironment )
            sku = value;

    }

    public Supplier GetSupplier ()
    { return supplier; }

    public void SetSupplier ( Supplier value )
    { supplier = value; }

    public Category? GetCategory ()
    { return category; }

    public void SetCategory ( Category? value )
    { category = value; }

    public Review[] GetReviews ()
    { return reviews; }

    public void SetReviews ( Review[] value )
    { reviews = value; }

    public PriceHistory[] GetPriceHistroy ()
    { return priceHistroy; }

    public void SetPriceHistroy ( PriceHistory[] value )
    { priceHistroy = value; }

    public Discount[] GetDiscounts ()
    { return discount; }

    public void SetDiscounts ( Discount[] value )
    { discount = value; }

    // Gets the price while taking discounts into account
    public decimal GetEffectivePrice ()
    {
        var activeDiscount = GetDiscounts ()?.FirstOrDefault(d => d.IsActive());
        if ( activeDiscount != null )
        {
            return price - ( price * activeDiscount.Discount_percentage / 100 );
        }
        return price;
    }

    public double GetAverageReviewRating ()
    {
        double rating = 0;

        if ( reviews == null )
        {
            return 0;
        }

        foreach ( Review review in reviews )
        {
            rating += review.Rating;
        }
        return rating / reviews.Length;
    }

    public void OrderStock ( int stock )
    {
        if ( stock < 0 )
        {
            stock = 0;
        }

        currentStock += stock;

        if ( currentStock > maxStock )
        {
            currentStock = maxStock;
        }
    }

    public override string ToString ()
    {
        return $"Name: {Name}, Price: {Price}, Max Stock: {Max_stock}, Current Stock: {Current_stock}, Status: {Status}, " +
               $"Category: {GetCategory ().Name}, Description: {Description}, SKU: {sku}, " +
               $"Supplier: {GetSupplier ().Name}, " +
               $"CreatedAt: {Created_at}, UpdatedAt: {Updated_at}";
    }

    public override bool Equals ( object? obj )
    {
        if ( obj == null || GetType () != obj.GetType () )
            return false;

        Product otherProduct = (Product) obj;
        return sku == otherProduct.sku;
    }

    public override int GetHashCode ()
    {
        return sku.GetHashCode ();
    }
}