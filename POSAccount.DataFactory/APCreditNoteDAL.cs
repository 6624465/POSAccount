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
    public class APCreditNoteDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public APCreditNoteDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<APCreditNote> GetList(short branchID)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTAPCREDITNOTE, MapBuilder<APCreditNote>.MapAllProperties()
                                                    .DoNotMap(d => d.APCreditNoteDetails)
                                                    .Build(), branchID).ToList();
        }

        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }




        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var apcreditnote = (APCreditNote)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEAPCREDITNOTE);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, apcreditnote.DocumentNo);
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, apcreditnote.DocumentDate);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, apcreditnote.BranchID);
                db.AddInParameter(savecommand, "ReferenceNo", System.Data.DbType.String, apcreditnote.ReferenceNo);
                db.AddInParameter(savecommand, "CreditorCode", System.Data.DbType.String, apcreditnote.CreditorCode);
                db.AddInParameter(savecommand, "CreditTerm", System.Data.DbType.String, apcreditnote.CreditTerm);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, apcreditnote.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, apcreditnote.ExchangeRate);
                db.AddInParameter(savecommand, "CreditorAccount", System.Data.DbType.String, apcreditnote.CreditorAccount);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, apcreditnote.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, apcreditnote.LocalAmount);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, apcreditnote.TaxAmount);
                db.AddInParameter(savecommand, "TotalAmount", System.Data.DbType.Decimal, apcreditnote.TotalAmount);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, apcreditnote.Remark);
                db.AddInParameter(savecommand, "Source", System.Data.DbType.String, apcreditnote.Source);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, apcreditnote.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, apcreditnote.ModifiedBy);
                db.AddOutParameter(savecommand, "NewDocumentNo", System.Data.DbType.String, 25);

                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    var apDetailsItemDAL = new APCreditNoteDetailDAL();
                    // Get the New Quotation No.
                    apcreditnote.DocumentNo = savecommand.Parameters["@NewDocumentNo"].Value.ToString();
                    apcreditnote.APCreditNoteDetails.ForEach(dt =>
                    {
                        dt.DocumentNo = apcreditnote.DocumentNo;
                        dt.CreatedBy = apcreditnote.CreatedBy;
                        dt.ModifiedBy = apcreditnote.ModifiedBy;
                    }
                        );
                    result = Convert.ToInt16(apDetailsItemDAL.Delete(apcreditnote.DocumentNo, transaction));
                    result = apDetailsItemDAL.SaveList(apcreditnote.APCreditNoteDetails, transaction) == true ? 1 : 0;

                }

                if (result > 0)
                    transaction.Commit();
                else
                    transaction.Rollback();


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
            var apcreditnote = (APCreditNote)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEAPCREDITNOTE);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, apcreditnote.DocumentNo);

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
            var item = ((APCreditNote)lookupItem);

            var apcreditnoteItem = db.ExecuteSprocAccessor(DBRoutine.SELECTAPCREDITNOTE,
                                                    MapBuilder<APCreditNote>.MapAllProperties()
                                                    .DoNotMap(d => d.APCreditNoteDetails)
                                                    .Build(),
                                                    item.DocumentNo).FirstOrDefault();

            if (apcreditnoteItem == null)
                return null;


            if (apcreditnoteItem != null)
            {
                apcreditnoteItem.APCreditNoteDetails = new POSAccount.DataFactory.APCreditNoteDetailDAL().GetListByDocumentNo(apcreditnoteItem.DocumentNo);
            }
            return apcreditnoteItem;
        }

        #endregion






    }
}

