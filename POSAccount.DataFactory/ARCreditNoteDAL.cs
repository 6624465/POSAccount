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
    public class ARCreditNoteDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ARCreditNoteDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<ARCreditNote> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTARCREDITNOTE, MapBuilder<ARCreditNote>.MapAllProperties()
                                                    .DoNotMap(d => d.ARCreditNoteDetails)
                                                    .Build()).ToList();
        }

        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }




        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var arcreditnote = (ARCreditNote)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEARCREDITNOTE);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, arcreditnote.DocumentNo);
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, arcreditnote.DocumentDate);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, arcreditnote.BranchID);
                db.AddInParameter(savecommand, "ReferenceNo", System.Data.DbType.String, arcreditnote.ReferenceNo);
                db.AddInParameter(savecommand, "DebtorCode", System.Data.DbType.String, arcreditnote.DebtorCode);
                db.AddInParameter(savecommand, "CreditTerm", System.Data.DbType.String, arcreditnote.CreditTerm);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, arcreditnote.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, arcreditnote.ExchangeRate);
                db.AddInParameter(savecommand, "DebtorAccount", System.Data.DbType.String, arcreditnote.DebtorAccount);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, arcreditnote.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, arcreditnote.LocalAmount);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, arcreditnote.TaxAmount);
                db.AddInParameter(savecommand, "TotalAmount", System.Data.DbType.Decimal, arcreditnote.TotalAmount);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, arcreditnote.Remark);
                db.AddInParameter(savecommand, "Source", System.Data.DbType.String, arcreditnote.Source);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, arcreditnote.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, arcreditnote.ModifiedBy);
                db.AddOutParameter(savecommand, "NewDocumentNo", System.Data.DbType.String, 25);

                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    var arDetailsItemDAL = new ARCreditNoteDetailDAL();
                    // Get the New Quotation No.
                    arcreditnote.DocumentNo = savecommand.Parameters["@NewDocumentNo"].Value.ToString();
                    arcreditnote.ARCreditNoteDetails.ForEach(dt =>
                    {
                        dt.DocumentNo = arcreditnote.DocumentNo;
                        dt.CreatedBy = arcreditnote.CreatedBy;
                        dt.ModifiedBy = arcreditnote.ModifiedBy;
                    }
                        );
                    result = Convert.ToInt16(arDetailsItemDAL.Delete(arcreditnote.DocumentNo, transaction));
                    result = arDetailsItemDAL.SaveList(arcreditnote.ARCreditNoteDetails, transaction) == true ? 1 : 0;

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
            var arcreditnote = (ARCreditNote)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEARCREDITNOTE);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, arcreditnote.DocumentNo);

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
            var item = ((ARCreditNote)lookupItem);

            var arcreditnoteItem = db.ExecuteSprocAccessor(DBRoutine.SELECTARCREDITNOTE,
                                                    MapBuilder<ARCreditNote>.MapAllProperties()
                                                    .DoNotMap(d => d.ARCreditNoteDetails)
                                                    .Build(),
                                                    item.DocumentNo).FirstOrDefault();
            if (arcreditnoteItem == null)
                return null;


            if (arcreditnoteItem != null)
            {
                arcreditnoteItem.ARCreditNoteDetails = new POSAccount.DataFactory.ARCreditNoteDetailDAL().GetListByDocumentNo(arcreditnoteItem.DocumentNo);
            }
            
            
            
            return arcreditnoteItem;
        }

        #endregion






    }
}

