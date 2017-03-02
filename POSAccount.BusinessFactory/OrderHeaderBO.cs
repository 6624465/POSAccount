using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSAccount.Contract;
using POSAccount.DataFactory;
using System.Data;

namespace POSAccount.BusinessFactory
{
    public class OrderHeaderBO
    {
        private OrderHeaderDAL OrderHeaderDAL;
        public OrderHeaderBO()
        {

            OrderHeaderDAL = new OrderHeaderDAL();
        }

        public List<OrderHeader> GetList()
        {
            return OrderHeaderDAL.GetList();
        }


        public bool SaveOrderHeader(OrderHeader newItem)
        {

            return OrderHeaderDAL.Save(newItem);

        }

        public bool DeleteOrderHeader(OrderHeader item)
        {
            return OrderHeaderDAL.Delete(item);
        }

        public OrderHeader GetOrderHeader(OrderHeader item)
        {
            return (OrderHeader)OrderHeaderDAL.GetItem<OrderHeader>(item);
        }

        public IDataReader PerformSearch(string whereclause)
        {
            return OrderHeaderDAL.PerformSearch(whereclause);
        }
    }
}
