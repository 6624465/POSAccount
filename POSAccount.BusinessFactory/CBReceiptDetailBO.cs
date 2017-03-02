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
    public class CBReceiptDetailBO
    {
        private CBReceiptDetailDAL cbreceiptdetailDAL;
        public CBReceiptDetailBO()
        {

            cbreceiptdetailDAL = new CBReceiptDetailDAL();
        }

        public List<CBReceiptDetail> GetList()
        {
            return cbreceiptdetailDAL.GetList();
        }


        public List<CBReceiptDetail> GetListByDocumentNo(string documentNo)
        {
            return cbreceiptdetailDAL.GetListByDocumentNo(documentNo);
        }


        public bool SaveCBReceiptDetail(CBReceiptDetail newItem)
        {

            return cbreceiptdetailDAL.Save(newItem);

        }

        public bool DeleteCBReceiptDetail(CBReceiptDetail item)
        {
            return cbreceiptdetailDAL.Delete(item);
        }

        public CBReceiptDetail GetCBReceiptDetail(CBReceiptDetail item)
        {
            return (CBReceiptDetail)cbreceiptdetailDAL.GetItem<CBReceiptDetail>(item);
        }

    }
}
