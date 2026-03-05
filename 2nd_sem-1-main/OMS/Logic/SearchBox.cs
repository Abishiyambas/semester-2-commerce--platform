using System.Collections.Generic;

namespace OMS.Logic;
using System.Linq;

public class Search_Box
{
    //Searches in a list of orders, for orders with the right order_id.
    public static List<Order> searched_orders(List<Order> orders, int orderId)
        {
            return orders.Where(o => o.GetOrderId == orderId).ToList();

        }
    
}