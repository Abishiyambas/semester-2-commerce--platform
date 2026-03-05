using System.Threading.Tasks;

namespace PIM_OMS_Interface
{
    public interface IPim
    {
        public Task<int> GetProductStock ( int productId );
        public Task<int> GetProductMaxStock ( int productId );
    }
}
