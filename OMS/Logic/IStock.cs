using System.Collections.Generic;

namespace OMS.Logic;

public interface  IStock
{ 
    int UpdateCurrentStock(Dictionary<int, int> stock);

}