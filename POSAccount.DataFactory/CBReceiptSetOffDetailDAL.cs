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
    public class CBReceiptSetOffDetailDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public CBReceiptSetOffDetailDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<CBReceiptSetOffDetail> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCBRECEIPTSETOFFDETAIL, MapBuilder<CBReceiptSetOffDetail>.BuildAllProperties()).ToList();
        }

        public List<CBReceiptSetOffDetail> GetListByDocumentNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCBRECEIPTSETOFFDETAIL, MapBuilder<CBReceiptSetOffDetail>.BuildAllProperties(), documentNo).ToList();
        }

        public List<CBReceiptSetOffDetail> GetDebtorOutStandingDocuments(string debtorCode, string matchDocumentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.GETDEBTOROUTSTANDINGDOCUMENTS, MapBuilder<CBReceiptSetOffDetail>
                                                                                        .MapAllProperties()
                                                                                        .DoNotMap(p => p.CreatedBy)
                                                                                        .DoNotMap(p => p.CreatedOn)
                                                                                        .DoNotMap(p => p.ModifiedBy)
                                                                                        .DoNotMap(p => p.ModifiedOn)
                                                                                        .Build(), debtorCode, matchDocumentNo.Length == 0 ? null : matchDocumentNo).ToList();
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

            var cbreceiptsetoffdetail = (CBReceiptSetOffDetail)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVECBRECEIPTSETOFFDETAIL);


                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, cbreceiptsetoffdetail.DocumentNo);
                db.AddInParameter(savecommand, "ItemNo", System.Data.DbType.Int16, cbreceiptsetoffdetail.ItemNo);
                db.AddInParameter(savecommand, "MatchDocumentType", System.Data.DbType.String, cbreceiptsetoffdetail.MatchDocumentType);
                db.AddInParameter(savecommand, "MatchDocumentNo", System.Data.DbType.String, cbreceiptsetoffdetail.MatchDocumentNo);
                db.AddInParameter(savecommand, "MatchDocumentDate", System.Data.DbType.DateTime, cbreceiptsetoffdetail.MatchDocumentDate);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, cbreceiptsetoffdetail.CurrencyCode);
                db.AddInParameter(savecommand, "CreditTermInDays", System.Data.DbType.Int16, cbreceiptsetoffdetail.CreditTermInDays);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, cbreceiptsetoffdetail.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, cbreceiptsetoffdetail.BaseAmount);
                db.AddInParameter(savecommand, "BaseApplyAmount", System.Data.DbType.Decimal, cbreceiptsetoffdetail.BaseApplyAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, cbreceiptsetoffdetail.LocalAmount);
                db.AddInParameter(savecommand, "LocalApplyAmount", System.Data.DbType.Decimal, cbreceiptsetoffdetail.LocalApplyAmount);
                db.AddInParameter(savecommand, "SetOffDate", System.Data.DbType.DateTime, cbreceiptsetoffdetail.SetOffDate);
                db.AddInParameter(savecommand, "DebtorCode", System.Data.DbType.String, cbreceiptsetoffdetail.DebtorCode);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, cbreceiptsetoffdetail.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, cbreceiptsetoffdetail.ModifiedBy);



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
            var cbreceiptsetoffdetailItem = new CBReceiptSetOffDetail { DocumentNo = documentNo };

            return Delete(cbreceiptsetoffdetailItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var cbreceiptsetoffdetail = (CBReceiptSetOffDetail)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECBRECEIPTSETOFFDETAIL);
                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, cbreceiptsetoffdetail.DocumentNo);
                db.AddInParameter(deleteCommand, "ItemNo", System.Data.DbType.Int16, cbreceiptsetoffdetail.ItemNo);

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
            var item = ((CBReceiptSetOffDetail)lookupItem);

            var cbreceiptsetoffdetailItem = db.ExecuteSprocAccessor(DBRoutine.SELECTCBRECEIPTSETOFFDETAIL,
                                                    MapBuilder<CBReceiptSetOffDetail>.BuildAllProperties(),
                                                    item.DocumentNo, item.ItemNo).FirstOrDefault();
            return cbreceiptsetoffdetailItem;
        }

        #endregion






    }
}

