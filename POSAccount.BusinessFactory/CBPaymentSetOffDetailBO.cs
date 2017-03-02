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
     
    public class CBPaymentSetOffDetailBO
    {
        private CBPaymentSetOffDetailDAL cbPaymentsetoffdetailDAL;
        public CBPaymentSetOffDetailBO()
        {

            cbPaymentsetoffdetailDAL = new CBPaymentSetOffDetailDAL();
        }

        public List<CBPaymentSetOffDetail> GetList()
        {
            return cbPaymentsetoffdetailDAL.GetList();
        }

        public List<CBPaymentSetOffDetail> GetCreditorOutStandingDocuments(string creditorCode, string matchDocumentNo)
        {
            return cbPaymentsetoffdetailDAL.GetCreditorOutStandingDocuments(creditorCode, matchDocumentNo);
        }


        public List<CBPaymentSetOffDetail> GetListByDocumentNo(string documentNo)
        {
            return cbPaymentsetoffdetailDAL.GetListByDocumentNo(documentNo);
        }


        public bool SaveCBPaymentDetail(CBPaymentSetOffDetail newItem)
        {

            return cbPaymentsetoffdetailDAL.Save(newItem);

        }

        public bool DeleteCBPaymentDetail(CBPaymentSetOffDetail item)
        {
            return cbPaymentsetoffdetailDAL.Delete(item);
        }

        public CBPaymentSetOffDetail GetCBPaymentSetOffDetail(CBPaymentSetOffDetail item)
        {
            return (CBPaymentSetOffDetail)cbPaymentsetoffdetailDAL.GetItem<CBPaymentSetOffDetail>(item);
        }

    }
}
