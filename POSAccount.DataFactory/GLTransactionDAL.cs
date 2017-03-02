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
    public class GLTransactionDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GLTransactionDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<GLTransaction> GetList(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTGLTRANSACTION, MapBuilder<GLTransaction>.BuildAllProperties(), documentNo).ToList();
        }


        public List<BankRecon> GetBankReconciliationList(short branchId, string bankCode, DateTime dateFrom, DateTime dateTo )
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTBANKRECONCILIATIONTRANSACTION, MapBuilder<BankRecon>
                                                                                    .MapAllProperties()
                                                                                    .DoNotMap(x=> x.BankCode)
                                                                                    .DoNotMap(x=> x.StartDate)
                                                                                    .DoNotMap(x=> x.EndDate)
                                                                                    .Build(),branchId,bankCode,dateFrom,dateTo).ToList();
        }


        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }

        public bool SaveList<T>(List<T> items, DbTransaction parentTransaction) where T : IContract
        {
            var result = true;

            if (items.Count == 0)
                result = true;

            foreach (var item in items)
            {
                result = Save(item, parentTransaction);
                if (result == false) break;
            }


            return result;

        }


        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var gltransaction = (GLTransaction)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);

            
            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEGLTRANSACTION);

                db.AddInParameter(savecommand, "TransactionNo", System.Data.DbType.String, gltransaction.TransactionNo);
                db.AddInParameter(savecommand, "ItemNo", System.Data.DbType.Int16, gltransaction.ItemNo);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, gltransaction.BranchID);
                db.AddInParameter(savecommand, "AccountCode", System.Data.DbType.String, gltransaction.AccountCode ?? "");
                db.AddInParameter(savecommand, "AccountDate", System.Data.DbType.DateTime, gltransaction.AccountDate);
                db.AddInParameter(savecommand, "Source", System.Data.DbType.String, gltransaction.Source);
                db.AddInParameter(savecommand, "DocumentType", System.Data.DbType.String, gltransaction.DocumentType??"");
                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, gltransaction.DocumentNo ?? "");
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, gltransaction.DocumentDate);
                db.AddInParameter(savecommand, "DebtorCode", System.Data.DbType.String, gltransaction.DebtorCode??"");
                db.AddInParameter(savecommand, "CreditorCode", System.Data.DbType.String, gltransaction.CreditorCode??"");
                db.AddInParameter(savecommand, "ChequeNo", System.Data.DbType.String, gltransaction.ChequeNo??"");
                db.AddInParameter(savecommand, "BankInSlipNo", System.Data.DbType.String, gltransaction.BankInSlipNo);
                db.AddInParameter(savecommand, "BankStatementPgNo", System.Data.DbType.Int16, gltransaction.BankStatementPgNo);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, gltransaction.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, gltransaction.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, gltransaction.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, gltransaction.LocalAmount);
                db.AddInParameter(savecommand, "CreditAmount", System.Data.DbType.Decimal, gltransaction.CreditAmount);
                db.AddInParameter(savecommand, "DebitAmount", System.Data.DbType.Decimal, gltransaction.DebitAmount);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, gltransaction.Remark==null? "":gltransaction.Remark);
                db.AddInParameter(savecommand, "BankStatementTotalPgNo", System.Data.DbType.Int16, gltransaction.BankStatementTotalPgNo);


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


        public bool Delete(string documentNo, DbTransaction parentTransaction)
        {
            var GLtransactionItem = new GLTransaction { DocumentNo = documentNo };

            return Delete(GLtransactionItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var gltransaction = (GLTransaction)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEGLTRANSACTION);
                db.AddInParameter(deleteCommand, "TransactionNo", System.Data.DbType.String, gltransaction.TransactionNo);
                db.AddInParameter(deleteCommand, "ItemNo", System.Data.DbType.Int16, gltransaction.ItemNo);


                result = Convert.ToBoolean(db.ExecuteNonQuery(deleteCommand, transaction));

                if (currentTransaction == null) 
                    transaction.Commit();

            }
            catch (Exception ex)
            {
                if (currentTransaction == null)
                    transaction.Rollback();
                
                throw ex;
            }

            return result;
        }

        public bool Delete(string documentNo, short branchID, DbTransaction parentTransaction)
        {
            var result = false;

            currentTransaction = parentTransaction;
            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEGLTRANSACTIONBYDOCUMENTNO);
                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, documentNo);
                db.AddInParameter(deleteCommand, "BranchID", System.Data.DbType.Int16, branchID);



                result = Convert.ToBoolean(db.ExecuteNonQuery(deleteCommand, transaction));

                if (currentTransaction == null)
                    transaction.Commit();

            }
            catch (Exception ex)
            {
                if (currentTransaction == null)
                    transaction.Rollback();
                
                throw ex;
            }

            return result;
        }

        public IContract GetItem<T>(IContract lookupItem) where T : IContract
        {
            var item = ((GLTransaction)lookupItem);

            var gltransactionItem = db.ExecuteSprocAccessor(DBRoutine.SELECTGLTRANSACTION,
                                                    MapBuilder<GLTransaction>.BuildAllProperties(),
                                                    item.TransactionNo,item.ItemNo).FirstOrDefault();
            return gltransactionItem;
        }

        #endregion






    }
}

