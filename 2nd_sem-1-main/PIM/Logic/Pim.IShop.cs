using System;
using System.Linq;
using System.Threading.Tasks;

namespace PIM_Logic
{
    public partial class Pim : PIM_SHOP_Interface.IPim // Implementation of the interface
    {
        async public Task<string> GetProductName ( int productId )
        {
            try
            {
                var product = await GetProduct(productId);
                return product?.Name ?? string.Empty;
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting product name: {ex.Message}" );
                return string.Empty;
            }
        }

        async public Task<string> GetProductDescription ( int productId )
        {
            try
            {
                var product = await GetProduct(productId);
                return product?.Description ?? string.Empty;
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting product description: {ex.Message}" );
                return string.Empty;
            }
        }

        async public Task<double> GetProductPrice ( int productId )
        {
            try
            {
                var product = await GetProduct(productId);
                if ( product is null )
                    return 0;
                return (double) product.GetEffectivePrice();
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting product price: {ex.Message}" );
                return -1;
            }
        }

        async public Task<double> GetProductPriceNormal ( int productId )
        {
            try
            {
                var product = await GetProduct(productId);
                if ( product is null )
                    return 0;
                return (double) product.Price;
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting product price: {ex.Message}" );
                return -1;
            }
        }

        async public Task<bool> GetProductStatus ( int productId )
        {
            try
            {
                var product = await GetProduct(productId);
                return product?.Status ?? false;
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting product stock: {ex.Message}" );
                return false;
            }
        }

        async public Task<int> GetProductStock ( int productId )
        {
            try
            {
                var product = await GetProduct(productId);
                return product?.Current_stock ?? 0;
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting product stock: {ex.Message}" );
                return 0;
            }
        }

        async public Task<string> GetProductCategory ( int productId )
        {
            try
            {
                var product = await GetProduct(productId);
                return product?.GetCategory ()?.Name ?? string.Empty;
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting product category: {ex.Message}" );
                return string.Empty;
            }
        }

        async public Task<string> GetProductSupplier ( int productId )
        {
            try
            {
                var product = await GetProduct(productId);
                return product?.GetSupplier ()?.Name ?? string.Empty;
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting product supplier: {ex.Message}" );
                return string.Empty;
            }
        }

        async public Task<Tuple<int, string>[]> GetAllProductReviews ( int productId )
        {
            try
            {
                var reviews = await GetReviews(productId);
                return reviews.Select ( review => Tuple.Create ( review.Rating, review.Comment ) ).ToArray ();
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting product reviews: {ex.Message}" );
                return Array.Empty<Tuple<int, string>> ();
            }
        }

        async public Task<string> GetProductSKU ( int productId )
        {
            try
            {
                return (await GetProduct ( productId )).GetSku() ?? string.Empty;
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting product reviews: {ex.Message}" );
                return string.Empty;
            }
        }

        async public Task<int[]> GetAllProductIds ()
        {
            try
            {
                var products = await GetAllProducts();
                return products.Select ( product => product.Product_id ).ToArray ();
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting all product IDs: {ex.Message}" );
                return Array.Empty<int> ();
            }
        }

        async public Task<int[]> GetAllProductIdsByCategory ( int categoryId )
        {
            try
            {
                var products = await GetAllProducts();
                return products.Where ( product => product.Category_id == categoryId ).Select ( product => product.Product_id ).ToArray ();
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting product IDs by category: {ex.Message}" );
                return Array.Empty<int> ();
            }
        }
    }
}