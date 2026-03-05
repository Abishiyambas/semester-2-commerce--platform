namespace OMS.Logic;
using System.Linq;
using System.Collections.Generic;


public class Sort_By_Date
{
    //sorts list of order by amount from oldest to newest.
    public static List<Order> SortByDate_Oldest_First(List<Order> orders)
        {
            return orders.OrderBy(o => o.GetOrderDate).ToList();
        }
    //sorts list of order by date from newest to oldest.

    public static List<Order> SortByDate_Newest_First(List<Order> orders)
        {
            return orders.OrderByDescending(o => o.GetOrderDate).ToList();
        }
   

}