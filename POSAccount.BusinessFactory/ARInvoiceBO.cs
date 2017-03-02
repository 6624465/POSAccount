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
    public class ARInvoiceBO
    {
        private ARInvoiceDAL arinvoiceDAL;
        public ARInvoiceBO()
        {

            arinvoiceDAL = new ARInvoiceDAL();
        }

        public List<ARInvoice> GetList()
        {
            return arinvoiceDAL.GetList();
        }

        public List<ARInvoice> GetListByDocumentNo(string documentNo)
        {
            return arinvoiceDAL.GetListByDocumentNo(documentNo);
        }
        
        public bool SaveARInvoice(ARInvoice newItem)
        {

            return arinvoiceDAL.Save(newItem);

        }

        public bool DeleteARInvoice(ARInvoice item)
        {
            return arinvoiceDAL.Delete(item);
        }

        public ARInvoice GetARInvoice(ARInvoice item)
        {
            return (ARInvoice)arinvoiceDAL.GetItem<ARInvoice>(item);
        }


        public IDataReader PerformSearch(string whereclause)
        {
            return arinvoiceDAL.PerformSearch(whereclause);
        }
    }
}
