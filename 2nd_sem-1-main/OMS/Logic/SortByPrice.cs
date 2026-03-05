using System.Collections.Generic;

namespace OMS.Logic;
using System.Linq;


public class Sort_By_Price
{ 
    //sorts list of order by price, from lowest to highest.
    public static List<Order> SortByPrice_Low_To_High(List<Order> orders)
        {
            return orders.OrderBy(o => o.GetPrice).ToList();
        }
    //sorts list of order by price, from highest to lowest.

    public static List<Order> SortByPrice_High_To_Low(List<Order> orders)
        {
            return orders.OrderByDescending(o => o.GetPrice).ToList();
        }
}