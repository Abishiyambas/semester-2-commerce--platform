using System;
using System.Threading.Tasks;

namespace PIM_Logic
{
    public partial class Pim : PIM_OMS_Interface.IPim
    {
        public async Task<int> GetProductMaxStock ( int productId )
        {
            try
            {
                var product = await GetProduct(productId);
                return product?.Current_stock ?? 0;
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"Error getting product max stock: {ex.Message}" );
                return 0;
            }
        }
    }
}