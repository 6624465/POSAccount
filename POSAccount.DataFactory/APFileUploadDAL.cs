using Microsoft.Practices.EnterpriseLibrary.Data;
using POSAccount.Contract;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace POSAccount.DataFactory
{
    public class APFileUploadDAL  
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;
        /// <summary>
        /// Constructor
        /// </summary>
        public APFileUploadDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<APFileUpload> GetList(short branchID)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTAPFILEUPLOAD, MapBuilder<APFileUpload>
                                                                        .MapAllProperties()
                                                                        .DoNotMap(dt=> dt.FileDetailList)
                                                                        .Build(),branchID).ToList();
        }

        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }

        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var apfileupload = (APFileUpload)(object)item;

            var connection = db.CreateConnection();
            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEAPFILEUPLOAD);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, apfileupload.DocumentNo);
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, apfileupload.DocumentDate);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, apfileupload.BranchID);
                db.AddInParameter(savecommand, "FileName", System.Data.DbType.String, apfileupload.FileName);
                db.AddInParameter(savecommand, "CreditorCode", System.Data.DbType.String, apfileupload.CreditorCode);
                db.AddInParameter(savecommand, "CreditTerm", System.Data.DbType.String, apfileupload.CreditTerm);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, apfileupload.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, apfileupload.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, apfileupload.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, apfileupload.LocalAmount);
                db.AddInParameter(savecommand, "DiscountAmount", System.Data.DbType.Decimal, apfileupload.DiscountAmount);
                db.AddInParameter(savecommand, "PaymentAmount", System.Data.DbType.Decimal, apfileupload.PaymentAmount);
                db.AddInParameter(savecommand, "IsVAT", System.Data.DbType.Boolean, apfileupload.IsVAT);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, apfileupload.TaxAmount);
                db.AddInParameter(savecommand, "TotalAmount", System.Data.DbType.Decimal, apfileupload.TotalAmount);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, apfileupload.Remark);
                db.AddInParameter(savecommand, "Source", System.Data.DbType.String, apfileupload.Source);
                db.AddInParameter(savecommand, "IsCancel", System.Data.DbType.Boolean, apfileupload.IsCancel);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, apfileupload.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, apfileupload.ModifiedBy);
                db.AddOutParameter(savecommand, "NewDocumentNo", System.Data.DbType.String, 25);

                result = db.ExecuteNonQuery(savecommand, transaction);



                if (result > 0)
                    transaction.Commit();
                else
                    transaction.Rollback();

            }
            catch (Exception ex)
            {
                if (currentTransaction == null)
                    transaction.Rollback();

                throw ex;
            }

            return (result > 0 ? true : false);
        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var apfileupload = (APFileUpload)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEAPFILEUPLOAD);

                

                db.AddInParameter(deleteCommand, "BranchID", System.Data.DbType.Int16, apfileupload.BranchID);
                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, apfileupload.DocumentNo);
                result = Convert.ToBoolean(db.ExecuteNonQuery(deleteCommand, transaction));

                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }

            return  result  ;
        }

        public IContract GetItem<T>(IContract lookupItem) where T : IContract
        {
            var obj = ((APFileUpload)lookupItem);

            var apfileuploadItem = db.ExecuteSprocAccessor(DBRoutine.SELECTAPFILEUPLOAD,
                                                    MapBuilder<APFileUpload>.MapAllProperties()
                                                                        .DoNotMap(dt => dt.FileDetailList)
                                                                        .Build(),obj.DocumentNo,obj.BranchID).FirstOrDefault();

            if (apfileuploadItem!=null)
            {

                apfileuploadItem.FileDetailList = new APFileDetailDAL().GetList(apfileuploadItem.DocumentNo);

            }


            return apfileuploadItem;
        }

        #endregion

    }
}

