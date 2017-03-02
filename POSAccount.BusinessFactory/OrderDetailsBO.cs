using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSAccount.Contract;
using POSAccount.DataFactory;

namespace POSAccount.BusinessFactory
{
    public class OrderDetailsBO
    {
        private OrderDetailsDAL OrderDetailsDAL;
        public OrderDetailsBO()
        {

            OrderDetailsDAL = new OrderDetailsDAL();
        }

        public List<OrderDetails> GetList()
        {
            return OrderDetailsDAL.GetList();
        }


        public bool SaveOrderDetails(OrderDetails newItem)
        {

            return OrderDetailsDAL.Save(newItem);

        }

        public bool DeleteOrderDetails(OrderDetails item)
        {
            return OrderDetailsDAL.Delete(item);
        }

        public OrderDetails GetOrderDetails(OrderDetails item)
        {
            return (OrderDetails)OrderDetailsDAL.GetItem<OrderDetails>(item);
        }


        public OrderDetails GetChargeCodePrice(string customerCode, string chargeCode)
        {
            return (OrderDetails)OrderDetailsDAL.GetChargeCodePrice(customerCode,  chargeCode);
        }
    }
}
