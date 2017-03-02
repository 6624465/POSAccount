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
    public class CBPaymentDetailDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public CBPaymentDetailDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<CBPaymentDetail> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCBPAYMENTDETAIL, MapBuilder<CBPaymentDetail>.BuildAllProperties()).ToList();
        }

        public List<CBPaymentDetail> GetListByDocumentNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCBPAYMENTDETAIL, MapBuilder<CBPaymentDetail>.BuildAllProperties(), documentNo).ToList();
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

            var cbpaymentdetail = (CBPaymentDetail)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVECBPAYMENTDETAIL);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, cbpaymentdetail.DocumentNo);
                db.AddInParameter(savecommand, "ItemNo", System.Data.DbType.Int16, cbpaymentdetail.ItemNo);
                db.AddInParameter(savecommand, "AccountCode", System.Data.DbType.String, cbpaymentdetail.AccountCode);
                db.AddInParameter(savecommand, "MatchDocumentType", System.Data.DbType.String, cbpaymentdetail.MatchDocumentType ?? "");
                db.AddInParameter(savecommand, "MatchDocumentNo", System.Data.DbType.String, cbpaymentdetail.MatchDocumentNo ?? "");
                db.AddInParameter(savecommand, "MatchDocumentDate", System.Data.DbType.DateTime, cbpaymentdetail.MatchDocumentDate);
                db.AddInParameter(savecommand, "CreditTermInDays", System.Data.DbType.Int16, cbpaymentdetail.CreditTermInDays);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, cbpaymentdetail.Remark);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, cbpaymentdetail.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, cbpaymentdetail.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, cbpaymentdetail.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, cbpaymentdetail.LocalAmount);
                db.AddInParameter(savecommand, "RequestNo", System.Data.DbType.String, cbpaymentdetail.RequestNo ?? "");
                db.AddInParameter(savecommand, "JobNo", System.Data.DbType.String, cbpaymentdetail.JobNo ?? "");
                db.AddInParameter(savecommand, "ChargeCode", System.Data.DbType.String, cbpaymentdetail.ChargeCode);
                db.AddInParameter(savecommand, "SetOffDate", System.Data.DbType.DateTime, cbpaymentdetail.SetOffDate);
                db.AddInParameter(savecommand, "WHPercent", System.Data.DbType.Int16, cbpaymentdetail.WHPercent);
                db.AddInParameter(savecommand, "WHAmount", System.Data.DbType.Decimal, cbpaymentdetail.WHAmount);
                




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
            var cbpaymentdetailItem = new CBPaymentDetail { DocumentNo = documentNo };

            return Delete(cbpaymentdetailItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var cbpaymentdetail = (CBPaymentDetail)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECBPAYMENTDETAIL);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, cbpaymentdetail.DocumentNo);
                db.AddInParameter(deleteCommand, "ItemNo", System.Data.DbType.Int16, cbpaymentdetail.ItemNo);

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
            var item = ((CBPaymentDetail)lookupItem);

            var cbpaymentdetailItem = db.ExecuteSprocAccessor(DBRoutine.SELECTCBPAYMENTDETAIL,
                                                    MapBuilder<CBPaymentDetail>.BuildAllProperties(),
                                                    item.DocumentNo, item.ItemNo).FirstOrDefault();
            return cbpaymentdetailItem;
        }

        #endregion






    }
}

