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
    public class CBReceiptSetOffDetailBO
    {
        private CBReceiptSetOffDetailDAL cbreceiptsetoffdetailDAL;
        public CBReceiptSetOffDetailBO()
        {

            cbreceiptsetoffdetailDAL = new CBReceiptSetOffDetailDAL();
        }

        public List<CBReceiptSetOffDetail> GetList()
        {
            return cbreceiptsetoffdetailDAL.GetList();
        }

        public List<CBReceiptSetOffDetail> GetDebtorOutStandingDocuments(string debtorCode, string matchDocumentNo)
        {
            return cbreceiptsetoffdetailDAL.GetDebtorOutStandingDocuments(debtorCode, matchDocumentNo);
        }


        public List<CBReceiptSetOffDetail> GetListByDocumentNo(string documentNo)
        {
            return cbreceiptsetoffdetailDAL.GetListByDocumentNo(documentNo);
        }


        public bool SaveCBReceiptDetail(CBReceiptSetOffDetail newItem)
        {

            return cbreceiptsetoffdetailDAL.Save(newItem);

        }

        public bool DeleteCBReceiptDetail(CBReceiptSetOffDetail item)
        {
            return cbreceiptsetoffdetailDAL.Delete(item);
        }

        public CBReceiptSetOffDetail GetCBReceiptSetOffDetail(CBReceiptSetOffDetail item)
        {
            return (CBReceiptSetOffDetail)cbreceiptsetoffdetailDAL.GetItem<CBReceiptSetOffDetail>(item);
        }

    }
}
