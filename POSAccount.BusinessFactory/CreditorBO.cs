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
    public class CreditorBO
    {
        private CreditorDAL creditorDAL;
        public CreditorBO()
        {

            creditorDAL = new CreditorDAL();
        }

        public List<Creditor> GetList()
        {
            return creditorDAL.GetList();
        }


        public bool SaveCreditor(Creditor newItem)
        {

            return creditorDAL.Save(newItem);

        }

        public bool DeleteCreditor(Creditor item)
        {
            return creditorDAL.Delete(item);
        }

        public Creditor GetCreditor(Creditor item)
        {
            return (Creditor)creditorDAL.GetItem<Creditor>(item);
        }

        public Address GetCreditorAddress(Creditor item)
        {
            return creditorDAL.GetCreditorAddress(item);
        }

        public IDataReader PerformSearch(string whereclause)
        {
            return creditorDAL.PerformSearch(whereclause);
        }
    }
}
