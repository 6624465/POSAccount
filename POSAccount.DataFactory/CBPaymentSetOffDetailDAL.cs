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
     
    public class CBPaymentSetOffDetailDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public CBPaymentSetOffDetailDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<CBPaymentSetOffDetail> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCBPAYMENTSETOFFDETAIL, MapBuilder<CBPaymentSetOffDetail>.BuildAllProperties()).ToList();
        }

        public List<CBPaymentSetOffDetail> GetListByDocumentNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCBPAYMENTSETOFFDETAIL, MapBuilder<CBPaymentSetOffDetail>.BuildAllProperties(), documentNo).ToList();
        }


        public List<CBPaymentSetOffDetail> GetCreditorOutStandingDocuments(string creditorCode, string matchDocumentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.GETCREDITOROUTSTANDINGDOCUMENTS, MapBuilder<CBPaymentSetOffDetail>
                                                                                        .MapAllProperties()
                                                                                        .DoNotMap(p => p.CreatedBy)
                                                                                        .DoNotMap(p => p.CreatedOn)
                                                                                        .DoNotMap(p => p.ModifiedBy)
                                                                                        .DoNotMap(p => p.ModifiedOn)
                                                                                        .Build(), creditorCode, matchDocumentNo.Length == 0 ? null : matchDocumentNo).ToList();
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

            var cbPaymentsetoffdetail = (CBPaymentSetOffDetail)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVECBPAYMENTSETOFFDETAIL);


                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, cbPaymentsetoffdetail.DocumentNo);
                db.AddInParameter(savecommand, "ItemNo", System.Data.DbType.Int16, cbPaymentsetoffdetail.ItemNo);
                db.AddInParameter(savecommand, "MatchDocumentType", System.Data.DbType.String, cbPaymentsetoffdetail.MatchDocumentType);
                db.AddInParameter(savecommand, "MatchDocumentNo", System.Data.DbType.String, cbPaymentsetoffdetail.MatchDocumentNo);
                db.AddInParameter(savecommand, "MatchDocumentDate", System.Data.DbType.DateTime, cbPaymentsetoffdetail.MatchDocumentDate);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, cbPaymentsetoffdetail.CurrencyCode);
                db.AddInParameter(savecommand, "CreditTermInDays", System.Data.DbType.Int16, cbPaymentsetoffdetail.CreditTermInDays);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, cbPaymentsetoffdetail.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, cbPaymentsetoffdetail.BaseAmount);
                db.AddInParameter(savecommand, "BaseApplyAmount", System.Data.DbType.Decimal, cbPaymentsetoffdetail.BaseApplyAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, cbPaymentsetoffdetail.LocalAmount);
                db.AddInParameter(savecommand, "LocalApplyAmount", System.Data.DbType.Decimal, cbPaymentsetoffdetail.LocalApplyAmount);
                db.AddInParameter(savecommand, "SetOffDate", System.Data.DbType.DateTime, cbPaymentsetoffdetail.SetOffDate);
                db.AddInParameter(savecommand, "CreditorCode", System.Data.DbType.String, cbPaymentsetoffdetail.CreditorCode);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, cbPaymentsetoffdetail.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, cbPaymentsetoffdetail.ModifiedBy);



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
            var cbPaymentsetoffdetailItem = new CBPaymentSetOffDetail { DocumentNo = documentNo };

            return Delete(cbPaymentsetoffdetailItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var cbPaymentsetoffdetail = (CBPaymentSetOffDetail)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECBPAYMENTSETOFFDETAIL);
                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, cbPaymentsetoffdetail.DocumentNo);
                db.AddInParameter(deleteCommand, "ItemNo", System.Data.DbType.Int16, cbPaymentsetoffdetail.ItemNo);

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
            var item = ((CBPaymentSetOffDetail)lookupItem);

            var cbPaymentsetoffdetailItem = db.ExecuteSprocAccessor(DBRoutine.SELECTCBPAYMENTSETOFFDETAIL,
                                                    MapBuilder<CBPaymentSetOffDetail>.BuildAllProperties(),
                                                    item.DocumentNo, item.ItemNo).FirstOrDefault();
            return cbPaymentsetoffdetailItem;
        }

        #endregion






    }
}
