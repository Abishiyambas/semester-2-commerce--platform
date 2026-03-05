using OMS.Logic;

namespace OMS.Data;

interface IOrder {
  //We assume productId and boughtAmount have the same length.
  Order MakeOrder(int customerId, int productId, int boughtAmount, int price); 
  
  (int, int) GetOrderConfirmation(); //returns orderId and trackingId.  

}
