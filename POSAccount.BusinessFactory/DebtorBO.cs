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
    public class DebtorBO
    {
        private DebtorDAL debtorDAL;
        public DebtorBO()
        {

            debtorDAL = new DebtorDAL();
        }

        public List<Debtor> GetList()
        {
            return debtorDAL.GetList();
        }

        public List<Debtor> GetListAutoSearch(string searchText)
        {
            return debtorDAL.GetListAutoSearch(searchText);
        }


        public bool SaveDebtor(Debtor newItem)
        {

            return debtorDAL.Save(newItem);

        }

        public bool DeleteDebtor(Debtor item)
        {
            return debtorDAL.Delete(item);
        }

        public Debtor GetDebtor(Debtor item)
        {
            return (Debtor)debtorDAL.GetItem<Debtor>(item);
        }

        public Address GetDebtorAddress(Debtor item)
        {
            return debtorDAL.GetDebtorAddress(item);
        }

        public IDataReader PerformSearch(string whereclause)
        {
            return debtorDAL.PerformSearch(whereclause);
        }
    }
}
