using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dapper;
using Npgsql;
using SHOP.Credentials;

namespace SHOP.Classes
{
    public class Review
    {
        private readonly Credentials.Credentials _credentials = new();

        public PrivateCustomer Reviewer { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Products Product { get; }

        public ObservableCollection<Review> ReviewList => ListInitializer.Instance.ReviewList;

        public Review(PrivateCustomer reviewer, string text, Products product)
        {
            Reviewer = reviewer;
            Text = text;
            Date = DateTime.Now;
            Product = product;
        }

        public Review() { } // Parameterless constructor for Dapper
        public void AddReviewToDatabase(Review review, int productId)
        {
            if (PrivateCustomer.LoggedInCustomer == null)
            {
                Console.WriteLine("Please log in.");
                return;
            }
            else if (review.Text == null)
            {
                Console.WriteLine("Please enter review text.");
                return;
            }
            

            const string insertReviewSql = @"
        INSERT INTO reviews (customer_id, product_id, created_on, text)
        VALUES (@CustomerId, @ProductId, @CreatedOn, @Text)";

            using var connection = new NpgsqlConnection(_credentials.Database);
            connection.Open();

            connection.Execute(insertReviewSql, new
            {
                CustomerId = PrivateCustomer.LoggedInCustomer.Id,
                ProductId = productId,
                CreatedOn = review.Date,
                Text = review.Text
            });

            Console.WriteLine("Review added.");
        }


        public List<Review> GetReviewsForProduct(int productId)
        {
            const string sql = @"
                SELECT r.created_on AS Date, r.text, c.id, c.name
                FROM reviews r
                JOIN customers c ON r.customer_id = c.id
                WHERE r.product_id = @ProductId
                ORDER BY r.created_on DESC";
            

            using var connection = new NpgsqlConnection(_credentials.Database);
            connection.Open();

            var reviews = connection.Query<Review, PrivateCustomer, Review>(
                sql,
                (review, customer) =>
                {
                    review.Reviewer = customer;
                    return review;
                },
                new { ProductId = productId },
                splitOn: "id"
            ).ToList();

            return reviews;
        }
    }
}
