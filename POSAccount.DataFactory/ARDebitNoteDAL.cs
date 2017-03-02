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
    public class ARDebitNoteDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ARDebitNoteDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<ARDebitNote> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTARDEBITNOTE, MapBuilder<ARDebitNote>.MapAllProperties()
                                                    .DoNotMap(d => d.ARDebitNoteDetails)
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

            var ardebitnote = (ARDebitNote)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEARDEBITNOTE);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, ardebitnote.DocumentNo);
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, ardebitnote.DocumentDate);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, ardebitnote.BranchID);
                db.AddInParameter(savecommand, "ReferenceNo", System.Data.DbType.String, ardebitnote.ReferenceNo);
                db.AddInParameter(savecommand, "DebtorCode", System.Data.DbType.String, ardebitnote.DebtorCode);
                db.AddInParameter(savecommand, "CreditTerm", System.Data.DbType.String, ardebitnote.CreditTerm);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, ardebitnote.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, ardebitnote.ExchangeRate);
                db.AddInParameter(savecommand, "DebtorAccount", System.Data.DbType.String, ardebitnote.DebtorAccount);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, ardebitnote.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, ardebitnote.LocalAmount);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, ardebitnote.TaxAmount);
                db.AddInParameter(savecommand, "TotalAmount", System.Data.DbType.Decimal, ardebitnote.TotalAmount);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, ardebitnote.Remark);
                db.AddInParameter(savecommand, "Source", System.Data.DbType.String, ardebitnote.Source);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, ardebitnote.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, ardebitnote.ModifiedBy);
                db.AddOutParameter(savecommand, "NewDocumentNo", System.Data.DbType.String, 25);

                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    var arDetailsItemDAL = new ARDebitNoteDetailDAL();
                    // Get the New Quotation No.
                    ardebitnote.DocumentNo = savecommand.Parameters["@NewDocumentNo"].Value.ToString();
                    ardebitnote.ARDebitNoteDetails.ForEach(dt =>
                    {
                        dt.DocumentNo = ardebitnote.DocumentNo;
                        dt.CreatedBy = ardebitnote.CreatedBy;
                        dt.ModifiedBy = ardebitnote.ModifiedBy;
                    }
                        );
                    result = Convert.ToInt16(arDetailsItemDAL.Delete(ardebitnote.DocumentNo, transaction));
                    result = arDetailsItemDAL.SaveList(ardebitnote.ARDebitNoteDetails, transaction) == true ? 1 : 0;

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
            var ardebitnote = (ARDebitNote)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEARDEBITNOTE);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, ardebitnote.DocumentNo);

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
            var item = ((ARDebitNote)lookupItem);

            var ardebitnoteItem = db.ExecuteSprocAccessor(DBRoutine.SELECTARDEBITNOTE,
                                                    MapBuilder<ARDebitNote>.MapAllProperties()
                                                    .DoNotMap(d => d.ARDebitNoteDetails)
                                                    .Build(),
                                                    item.DocumentNo).FirstOrDefault();

            if (ardebitnoteItem == null)
                return null;


            if (ardebitnoteItem != null)
            {
                ardebitnoteItem.ARDebitNoteDetails = new POSAccount.DataFactory.ARDebitNoteDetailDAL().GetListByDocumentNo(ardebitnoteItem.DocumentNo);
            }
            
            
            return ardebitnoteItem;
        }

        #endregion






    }
}

