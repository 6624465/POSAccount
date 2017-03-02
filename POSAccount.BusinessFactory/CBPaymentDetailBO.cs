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
    public class CBPaymentDetailBO
    {
        private CBPaymentDetailDAL cbpaymentdetailDAL;
        public CBPaymentDetailBO()
        {

            cbpaymentdetailDAL = new CBPaymentDetailDAL();
        }

        public List<CBPaymentDetail> GetList()
        {
            return cbpaymentdetailDAL.GetList();
        }

        public List<CBPaymentDetail> GetListByDocumentNo(string documentNo )
        {
            return cbpaymentdetailDAL.GetListByDocumentNo(documentNo);
        }
        

        public bool SaveCBPaymentDetail(CBPaymentDetail newItem)
        {

            return cbpaymentdetailDAL.Save(newItem);

        }

        public bool DeleteCBPaymentDetail(CBPaymentDetail item)
        {
            return cbpaymentdetailDAL.Delete(item);
        }

        public CBPaymentDetail GetCBPaymentDetail(CBPaymentDetail item)
        {
            return (CBPaymentDetail)cbpaymentdetailDAL.GetItem<CBPaymentDetail>(item);
        }

    }
}
