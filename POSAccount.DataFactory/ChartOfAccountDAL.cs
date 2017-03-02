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



namespace POSAccount.DataFactory
{
    public class ChartOfAccountDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChartOfAccountDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<ChartOfAccount> GetList(short branchID)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCHARTOFACCOUNT, MapBuilder<ChartOfAccount>.BuildAllProperties(),branchID).ToList();
        }

        public List<ChartOfAccount> GetCashBankAccountList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCASHBANKACCOUNT, MapBuilder<ChartOfAccount>.BuildAllProperties()).ToList();
        }

        public List<ChartOfAccount> GetListByAccountGroup(string accountGroup, short branchID)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCHARTOFACCOUNTBYACCOUNTGROUP, MapBuilder<ChartOfAccount>.BuildAllProperties(), accountGroup, branchID).ToList();
        }


        
        public List<ChartOfAccount> GetCreditorAccountList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCREDITORACCOUNT, MapBuilder<ChartOfAccount>.BuildAllProperties()).ToList();
        }

        public List<ChartOfAccount> GetDebtorAccountList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTDEBTORACCOUNT, MapBuilder<ChartOfAccount>.BuildAllProperties()).ToList();
        }

        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }




        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var chartofaccount = (ChartOfAccount)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVECHARTOFACCOUNT);

                db.AddInParameter(savecommand, "AccountCode", System.Data.DbType.String, chartofaccount.AccountCode);
                db.AddInParameter(savecommand, "Description", System.Data.DbType.String, chartofaccount.Description);
                db.AddInParameter(savecommand, "Description2", System.Data.DbType.String, chartofaccount.Description2==null? "":chartofaccount.Description2);
                db.AddInParameter(savecommand, "AccountGroup", System.Data.DbType.String, chartofaccount.AccountGroup);
                db.AddInParameter(savecommand, "DebitCredit", System.Data.DbType.String, chartofaccount.DebitCredit);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, chartofaccount.CurrencyCode);
                db.AddInParameter(savecommand, "Status", System.Data.DbType.Boolean, chartofaccount.Status);
                db.AddInParameter(savecommand, "Sequence", System.Data.DbType.Int16, chartofaccount.Sequence);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, chartofaccount.BranchID);

                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, chartofaccount.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, chartofaccount.ModifiedBy);


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
            var chartofaccount = (ChartOfAccount)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECHARTOFACCOUNT);



                db.AddInParameter(deleteCommand, "AccountCode", System.Data.DbType.String, chartofaccount.AccountCode);
                db.AddInParameter(deleteCommand, "BranchID", System.Data.DbType.Int16, chartofaccount.BranchID);

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

        public IContract GetItem<T>(IContract lookupItem) where T : IContract
        {
            var item = ((ChartOfAccount)lookupItem);

            var chartofaccountItem = db.ExecuteSprocAccessor(DBRoutine.SELECTCHARTOFACCOUNT,
                                                    MapBuilder<ChartOfAccount>.BuildAllProperties(),
                                                    item.AccountCode,item.BranchID).FirstOrDefault();
            return chartofaccountItem;
        }

        #endregion







         
    }
}

