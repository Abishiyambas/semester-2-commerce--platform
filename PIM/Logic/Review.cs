namespace PIM_Logic;

public class Review
{
    private int reviewId;
    private string comment;
    private int rating;
    private int productId;

    public int Review_id { get { return reviewId; } }
    public string Comment { get { return comment; } }
    public int Rating { get { return rating; } }
    public int Product_id { get { return productId; } }

    public Review ( int reviewId, string comment, int rating )  // If review is instaniated into a product there is no need to set productId 
    {
        this.reviewId = reviewId;
        this.comment = comment;
        this.rating = rating;
    }

    public Review ( string comment, int rating, int productId )
    {
        this.comment = comment;
        this.rating = rating;
        this.productId = productId;
    }

    public Review (int reviewId, string comment, int rating, int productId )
    {
        this.reviewId = reviewId;
        this.comment = comment;
        this.rating = rating;
        this.productId = productId;
    }

    public Review ( Npgsql.NpgsqlDataReader reader )
    {
        this.reviewId = reader.GetInt32 ( 0 );
        this.comment = reader.GetString ( 1 );
        this.rating = reader.GetInt32 ( 2 );
        this.productId = reader.GetInt32 ( 3 );
    }
}