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
    public class ChargeCodeDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChargeCodeDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<ChargeCodeMaster> GetList(short branchID)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCHARGECODE, MapBuilder<ChargeCodeMaster>.BuildAllProperties(),branchID).ToList();
        }

        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }




        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var chargecode = (ChargeCodeMaster)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVECHARGECODE);

                db.AddInParameter(savecommand, "ChargeCode", System.Data.DbType.String, chargecode.ChargeCode);
                db.AddInParameter(savecommand, "Description", System.Data.DbType.String, chargecode.Description??"");
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, chargecode.BranchID); 
                db.AddInParameter(savecommand, "BillingUnit", System.Data.DbType.String, chargecode.BillingUnit == null ? "" : chargecode.BillingUnit);
                db.AddInParameter(savecommand, "CreditTerm", System.Data.DbType.String, chargecode.CreditTerm == null ? "" : chargecode.CreditTerm);
                db.AddInParameter(savecommand, "PaymentTerm", System.Data.DbType.String, chargecode.PaymentTerm == null ? "" : chargecode.PaymentTerm);
                db.AddInParameter(savecommand, "VAT", System.Data.DbType.Decimal, chargecode.VAT);
                db.AddInParameter(savecommand, "VATAccountCode", System.Data.DbType.String, chargecode.VATAccountCode == null ? "" : chargecode.WHAccountCode);
                db.AddInParameter(savecommand, "WithHoldingTaxRate", System.Data.DbType.Decimal, chargecode.WithHoldingTaxRate);
                db.AddInParameter(savecommand, "WHAccountCode", System.Data.DbType.String, chargecode.WHAccountCode==null? "":chargecode.WHAccountCode);
                db.AddInParameter(savecommand, "AccountCode", System.Data.DbType.String, chargecode.AccountCode);
                db.AddInParameter(savecommand, "Status", System.Data.DbType.Boolean, chargecode.Status);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, chargecode.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, chargecode.ModifiedBy);




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
            var chargecode = (ChargeCodeMaster)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECHARGECODE);

                db.AddInParameter(deleteCommand, "ChargeCode", System.Data.DbType.String, chargecode.ChargeCode);
                db.AddInParameter(deleteCommand, "BranchID", System.Data.DbType.Int16, chargecode.BranchID);

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
            var item = ((ChargeCodeMaster)lookupItem);

            var chargecodeItem = db.ExecuteSprocAccessor(DBRoutine.SELECTCHARGECODE,
                                                    MapBuilder<ChargeCodeMaster>.BuildAllProperties(),
                                                    item.ChargeCode,item.BranchID).FirstOrDefault();
            return chargecodeItem;
        }

        #endregion






    }
}

