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
    public class CBReceiptGlDetailDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public CBReceiptGlDetailDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<CBReceiptGlDetail> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCBRECEIPTGLDETAIL, MapBuilder<CBReceiptGlDetail>.BuildAllProperties()).ToList();
        }

        public List<CBReceiptGlDetail> GetListByDocumentNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCBRECEIPTGLDETAIL, MapBuilder<CBReceiptGlDetail>.BuildAllProperties(), documentNo).ToList();
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

            var cbreceiptgldetail = (CBReceiptGlDetail)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVECBRECEIPTGLDETAIL);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, cbreceiptgldetail.DocumentNo);
                db.AddInParameter(savecommand, "ItemNo", System.Data.DbType.Int32, cbreceiptgldetail.ItemNo);
                db.AddInParameter(savecommand, "TransactionType", System.Data.DbType.String, cbreceiptgldetail.TransactionType);
                db.AddInParameter(savecommand, "AccountCode", System.Data.DbType.String, cbreceiptgldetail.AccountCode);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, cbreceiptgldetail.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, cbreceiptgldetail.LocalAmount);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, cbreceiptgldetail.TaxAmount);
                db.AddInParameter(savecommand, "TotalAmount", System.Data.DbType.Decimal, cbreceiptgldetail.TotalAmount);




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
            var CBReceiptGlDetailItem = new CBReceiptGlDetail { DocumentNo = documentNo };

            return Delete(CBReceiptGlDetailItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var cbreceiptgldetail = (CBReceiptGlDetail)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECBRECEIPTGLDETAIL);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, cbreceiptgldetail.DocumentNo);
                db.AddInParameter(deleteCommand, "ItemNo", System.Data.DbType.Int16, cbreceiptgldetail.ItemNo);

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
            var item = ((CBReceiptGlDetail)lookupItem);

            var CBReceiptGlDetailItem = db.ExecuteSprocAccessor(DBRoutine.SELECTCBRECEIPTGLDETAIL,
                                                    MapBuilder<CBReceiptGlDetail>.BuildAllProperties(),
                                                    item.DocumentNo, item.ItemNo).FirstOrDefault();
            return CBReceiptGlDetailItem;
        }

        #endregion






    }
}

