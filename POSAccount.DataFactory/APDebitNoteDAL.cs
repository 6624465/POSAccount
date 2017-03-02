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
    public class APDebitNoteDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public APDebitNoteDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<APDebitNote> GetList(short branchID)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTAPDEBITNOTE, MapBuilder<APDebitNote>.MapAllProperties()
                                                    .DoNotMap(d => d.APDebitNoteDetails)
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

            var apdebitnote = (APDebitNote)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEAPDEBITNOTE);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, apdebitnote.DocumentNo);
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, apdebitnote.DocumentDate);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, apdebitnote.BranchID);
                db.AddInParameter(savecommand, "ReferenceNo", System.Data.DbType.String, apdebitnote.ReferenceNo);
                db.AddInParameter(savecommand, "CreditorCode", System.Data.DbType.String, apdebitnote.CreditorCode);
                db.AddInParameter(savecommand, "CreditTerm", System.Data.DbType.String, apdebitnote.CreditTerm);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, apdebitnote.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, apdebitnote.ExchangeRate);
                db.AddInParameter(savecommand, "CreditorAccount", System.Data.DbType.String, apdebitnote.CreditorAccount);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, apdebitnote.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, apdebitnote.LocalAmount);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, apdebitnote.TaxAmount);
                db.AddInParameter(savecommand, "TotalAmount", System.Data.DbType.Decimal, apdebitnote.TotalAmount);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, apdebitnote.Remark);
                db.AddInParameter(savecommand, "Source", System.Data.DbType.String, apdebitnote.Source);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, apdebitnote.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, apdebitnote.ModifiedBy);
                db.AddOutParameter(savecommand, "NewDocumentNo", System.Data.DbType.String, 25);

                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    var apDetailsItemDAL = new APDebitNoteDetailDAL();
                    // Get the New Quotation No.
                    apdebitnote.DocumentNo = savecommand.Parameters["@NewDocumentNo"].Value.ToString();
                    apdebitnote.APDebitNoteDetails.ForEach(dt =>
                    {
                        dt.DocumentNo = apdebitnote.DocumentNo;
                        dt.CreatedBy = apdebitnote.CreatedBy;
                        dt.ModifiedBy = apdebitnote.ModifiedBy;
                    }
                        );


                    var lstSaveItems = apdebitnote.APDebitNoteDetails.Where(dt => dt.Status == true).ToList();

                    result = Convert.ToInt16(apDetailsItemDAL.Delete(apdebitnote.DocumentNo, transaction));


                    if (lstSaveItems != null && lstSaveItems.Count > 0)
                    {

                        result = apDetailsItemDAL.SaveList(lstSaveItems, transaction) == true ? 1 : 0;
                    }



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
            var apdebitnote = (APDebitNote)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEAPDEBITNOTE);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, apdebitnote.DocumentNo);

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
            var item = ((APDebitNote)lookupItem);

            var apdebitnoteItem = db.ExecuteSprocAccessor(DBRoutine.SELECTAPDEBITNOTE,
                                                    MapBuilder<APDebitNote>.MapAllProperties()
                                                    .DoNotMap(d => d.APDebitNoteDetails)
                                                    .Build(),
                                                    item.DocumentNo).FirstOrDefault();
            if (apdebitnoteItem == null)
                return null;


            if (apdebitnoteItem != null)
            {
                apdebitnoteItem.APDebitNoteDetails = new POSAccount.DataFactory.APDebitNoteDetailDAL().GetListByDocumentNo(apdebitnoteItem.DocumentNo);
            }

            return apdebitnoteItem;
        }

        #endregion






    }
}

