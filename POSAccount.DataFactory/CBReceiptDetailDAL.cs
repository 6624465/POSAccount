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
    public class CBReceiptDetailDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public CBReceiptDetailDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<CBReceiptDetail> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCBRECEIPTDETAIL, MapBuilder<CBReceiptDetail>.BuildAllProperties()).ToList();
        }

        public List<CBReceiptDetail> GetListByDocumentNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCBRECEIPTDETAIL, MapBuilder<CBReceiptDetail>.BuildAllProperties(), documentNo).ToList();
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

            var cbreceiptdetail = (CBReceiptDetail)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVECBRECEIPTDETAIL);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, cbreceiptdetail.DocumentNo);
                db.AddInParameter(savecommand, "ItemNo", System.Data.DbType.Int16, cbreceiptdetail.ItemNo);
                db.AddInParameter(savecommand, "AccountCode", System.Data.DbType.String, cbreceiptdetail.AccountCode ?? "");
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, cbreceiptdetail.Remark ?? "");
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, cbreceiptdetail.CurrencyCode ?? "");
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, cbreceiptdetail.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, cbreceiptdetail.BaseAmount);
                db.AddInParameter(savecommand, "BaseApplyAmount", System.Data.DbType.Decimal, cbreceiptdetail.BaseApplyAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, cbreceiptdetail.LocalAmount);
                db.AddInParameter(savecommand, "LocalApplyAmount", System.Data.DbType.Decimal, cbreceiptdetail.LocalApplyAmount);
                db.AddInParameter(savecommand, "MatchDocumentType", System.Data.DbType.String, cbreceiptdetail.MatchDocumentType ?? "");
                db.AddInParameter(savecommand, "MatchDocumentNo", System.Data.DbType.String, cbreceiptdetail.MatchDocumentNo ?? "");
                db.AddInParameter(savecommand, "MatchDocumentDate", System.Data.DbType.DateTime, cbreceiptdetail.MatchDocumentDate);
                db.AddInParameter(savecommand, "CreditTermInDays", System.Data.DbType.Int16, cbreceiptdetail.CreditTermInDays);
                db.AddInParameter(savecommand, "SetOffDate", System.Data.DbType.DateTime, cbreceiptdetail.SetOffDate);
                db.AddInParameter(savecommand, "DebtorCode", System.Data.DbType.String, cbreceiptdetail.DebtorCode ?? "");




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
            var cbreceiptdetailItem = new CBReceiptDetail { DocumentNo = documentNo };

            return Delete(cbreceiptdetailItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var cbreceiptdetail = (CBReceiptDetail)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECBRECEIPTDETAIL);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, cbreceiptdetail.DocumentNo);
                db.AddInParameter(deleteCommand, "ItemNo", System.Data.DbType.Int16, cbreceiptdetail.ItemNo);

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
            var item = ((CBReceiptDetail)lookupItem);

            var cbreceiptdetailItem = db.ExecuteSprocAccessor(DBRoutine.SELECTCBRECEIPTDETAIL,
                                                    MapBuilder<CBReceiptDetail>.BuildAllProperties(),
                                                    item.DocumentNo, item.ItemNo).FirstOrDefault();

            
            
            
            return cbreceiptdetailItem;
        }

        #endregion






    }
}

