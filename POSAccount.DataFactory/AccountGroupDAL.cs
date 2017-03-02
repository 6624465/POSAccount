using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using POSAccount.Contract;
using System.Data.Common;
using POSAccount.Contract;



namespace POSAccount.DataFactory
{
    public class AccountGroupDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountGroupDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<AccountGroup> GetList(short branchID)
        {
            var lstAccountGroup = db.ExecuteSprocAccessor(DBRoutine.LISTACCOUNTGROUP, MapBuilder<AccountGroup>.BuildAllProperties()).ToList();

            foreach (var accountGroupItem in lstAccountGroup)
            {
                accountGroupItem.COAList = new POSAccount.DataFactory.ChartOfAccountDAL().GetListByAccountGroup(accountGroupItem.Code, branchID).ToList();
            }

            return lstAccountGroup;

        }

        public List<AccountGroup> GetList()
        {
            var lstAccountGroup = db.ExecuteSprocAccessor(DBRoutine.LISTACCOUNTGROUP, MapBuilder<AccountGroup>.BuildAllProperties()).ToList();

             

            return lstAccountGroup;

        }


        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }




        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var accountgroup = (AccountGroup)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEACCOUNTGROUP);

                db.AddInParameter(savecommand, "AccountGroup", System.Data.DbType.String, accountgroup.Code);
                db.AddInParameter(savecommand, "AccountType", System.Data.DbType.String, accountgroup.AccountType);
                db.AddInParameter(savecommand, "Description", System.Data.DbType.String, accountgroup.Description);
                db.AddInParameter(savecommand, "Description1", System.Data.DbType.String, accountgroup.Description1);
                db.AddInParameter(savecommand, "Description2", System.Data.DbType.String, accountgroup.Description2);
                db.AddInParameter(savecommand, "SequenceNo", System.Data.DbType.Int16, accountgroup.SequenceNo);




                result = db.ExecuteNonQuery(savecommand, transaction);

                if (currentTransaction == null)
                    transaction.Commit();

            }
            catch (Exception)
            {
                if (currentTransaction == null)
                    transaction.Rollback();

                throw;
            }

            return (result > 0 ? true : false);

        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var accountgroup = (AccountGroup)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEACCOUNTGROUP);

                db.AddInParameter(deleteCommand, "AccountGroup", System.Data.DbType.String, accountgroup.Code);

                result = Convert.ToBoolean(db.ExecuteNonQuery(deleteCommand, transaction));

                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }

            return result;
        }

        public IContract GetItem<T>(IContract lookupItem,short branchID) where T : IContract
        {
            var item = ((AccountGroup)lookupItem);

            var accountgroupItem = db.ExecuteSprocAccessor(DBRoutine.SELECTACCOUNTGROUP,
                                                    MapBuilder<AccountGroup>.BuildAllProperties(),
                                                    ((AccountGroup)lookupItem).Code).FirstOrDefault();

            if (accountgroupItem != null)
            {
                accountgroupItem.COAList = new POSAccount.DataFactory.ChartOfAccountDAL().GetListByAccountGroup(accountgroupItem.Code,branchID).ToList();
            }

            return accountgroupItem;
        }

        #endregion






    }
}

