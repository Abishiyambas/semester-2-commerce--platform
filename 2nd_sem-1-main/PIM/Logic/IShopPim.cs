using System;
using System.Threading.Tasks;

namespace PIM_SHOP_Interface
{
    public interface IPim
    {
        public Task<string> GetProductName (int productId);
        public Task<string> GetProductDescription ( int productId );

        public Task<double> GetProductPrice ( int productId );          // Price after discounts if any
        public Task<double> GetProductPriceNormal ( int productId );    // Price with no discounts

        public Task<int> GetProductStock ( int productId );

        public Task<bool> GetProductStatus ( int productId );           // Is the product currenly being sold
        public Task<string> GetProductSKU ( int productId );
        public Task<string> GetProductCategory ( int productId );
        public Task<string> GetProductSupplier ( int productId );
        public Task<Tuple<int, string>[]> GetAllProductReviews ( int productId ); // output will be [ [4, "very nice .."], [2, "not good prod.."] ]
        
        public Task<int[]> GetAllProductIds ();
        public Task<int[]> GetAllProductIdsByCategory (int categoryId);
    }
}