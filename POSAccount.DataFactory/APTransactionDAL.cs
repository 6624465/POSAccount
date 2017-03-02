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
    public class APTransactionDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public APTransactionDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<APTransaction> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTAPTRANSACTION, MapBuilder<APTransaction>.BuildAllProperties()).ToList();
        }

        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }




        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var aptransaction = (APTransaction)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEAPTRANSACTION);


                db.AddInParameter(savecommand, "CreditorCode", System.Data.DbType.String, aptransaction.CreditorCode);
                db.AddInParameter(savecommand, "MatchDocumentType", System.Data.DbType.String, aptransaction.MatchDocumentType);
                db.AddInParameter(savecommand, "MatchDocumentNo", System.Data.DbType.String, aptransaction.MatchDocumentNo);
                db.AddInParameter(savecommand, "DocumentType", System.Data.DbType.String, aptransaction.DocumentType);
                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, aptransaction.DocumentNo);
                db.AddInParameter(savecommand, "DocumentTypeSequence", System.Data.DbType.Int16, aptransaction.DocumentTypeSequence);
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, aptransaction.DocumentDate);
                db.AddInParameter(savecommand, "CreditTermInDays", System.Data.DbType.Int16, aptransaction.CreditTermInDays);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, aptransaction.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, aptransaction.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, aptransaction.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, aptransaction.LocalAmount);
                db.AddInParameter(savecommand, "AccountDate", System.Data.DbType.DateTime, aptransaction.AccountDate);



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
            var aptransaction = (APTransaction)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEAPTRANSACTION);

                db.AddInParameter(deleteCommand, "CreditorCode", System.Data.DbType.String, aptransaction.CreditorCode);
                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, aptransaction.DocumentNo);
                db.AddInParameter(deleteCommand, "DocumentType", System.Data.DbType.String, aptransaction.DocumentType);
                db.AddInParameter(deleteCommand, "MatchDocumentNo", System.Data.DbType.String, aptransaction.MatchDocumentNo);
                db.AddInParameter(deleteCommand, "MatchDocumentType", System.Data.DbType.String, aptransaction.MatchDocumentType);


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
            var item = ((APTransaction)lookupItem);

            var aptransactionItem = db.ExecuteSprocAccessor(DBRoutine.SELECTAPTRANSACTION,
                                                    MapBuilder<APTransaction>.BuildAllProperties(),
                                                    item.CreditorCode,item.MatchDocumentType,item.MatchDocumentNo,item.DocumentNo,item.DocumentType).FirstOrDefault();
            return aptransactionItem;
        }

        #endregion






    }
}

