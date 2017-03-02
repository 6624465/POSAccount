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
    public class ARInvoiceDetailBO
    {
        private ARInvoiceDetailDAL ARInvoiceDetailDAL;
        public ARInvoiceDetailBO()
        {

            ARInvoiceDetailDAL = new ARInvoiceDetailDAL();
        }

        public List<ARInvoiceDetail> GetList()
        {
            return ARInvoiceDetailDAL.GetList();
        }

        public List<ARInvoiceDetail> GetListByDocumentNo(string documentNo)
        {
            return ARInvoiceDetailDAL.GetListByDocumentNo(documentNo);
        }

        public bool SaveARInvoiceDetail(ARInvoiceDetail newItem)
        {

            return ARInvoiceDetailDAL.Save(newItem);

        }

        public bool DeleteARInvoiceDetail(ARInvoiceDetail item)
        {
            return ARInvoiceDetailDAL.Delete(item);
        }

        public ARInvoiceDetail GetARInvoiceDetail(ARInvoiceDetail item)
        {
            return (ARInvoiceDetail)ARInvoiceDetailDAL.GetItem<ARInvoiceDetail>(item);
        }

    }
}
