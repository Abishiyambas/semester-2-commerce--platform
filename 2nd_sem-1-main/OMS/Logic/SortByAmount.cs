namespace OMS.Logic;
using System.Collections.Generic;
using System.Linq;


public class Sort_By_Amount
{
   
    //sorts list of order by amount from highest to lowest.
     public static List<Order> SortByAmount_Low_To_High(List<Order> orders)
        {
             return orders.OrderBy(o => o.GetAmount).ToList();
        }
     //sorts list of order by amount from lowest to highest.

     public static List<Order> SortByAmount_High_To_low(List<Order> orders)
        {
             return orders.OrderByDescending(o => o.GetAmount).ToList();
        }
 

}