using System.Threading.Tasks;

namespace PIM_DAM_Interface
{
    public interface IPim
    {
        public Task<int[]> GetAllProductIds ();
        public Task<string> GetProductName ( int productId );
    }
}