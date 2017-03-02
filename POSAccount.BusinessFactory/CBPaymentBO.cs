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
    public class CBPaymentBO
    {
        private CBPaymentDAL cbpaymentDAL;
        public CBPaymentBO()
        {

            cbpaymentDAL = new CBPaymentDAL();
        }

        public List<CBPayment> GetList()
        {
            return cbpaymentDAL.GetList();
        }


        public bool SaveCBPayment(CBPayment newItem)
        {

            return cbpaymentDAL.Save(newItem);

        }


        public bool DeleteCBPayment(string documentNo, string cancelledBy)
        {
            return cbpaymentDAL.Delete(documentNo, cancelledBy);
        }

        


        public bool DeleteCBPayment(CBPayment item)
        {
            return cbpaymentDAL.Delete(item);
        }

        public CBPayment GetCBPayment(CBPayment item)
        {
            return (CBPayment)cbpaymentDAL.GetItem<CBPayment>(item);
        }

        public IDataReader PerformSearch(string whereclause)
        {
            return cbpaymentDAL.PerformSearch(whereclause);
        }
    }
}
