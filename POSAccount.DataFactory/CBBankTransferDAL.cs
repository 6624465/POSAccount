using Microsoft.Practices.EnterpriseLibrary.Data;
using POSAccount.Contract;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace POSAccount.DataFactory
{
    public class CBBankTransferDAL
    {
        private Database db;

        /// <summary>
        /// Constructor
        /// </summary>
        public CBBankTransferDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<CBBankTransfer> GetList(short branchID)
        {
            return db.ExecuteSprocAccessor(DBRoutine.SELECTCBBANKTRANSFER, MapBuilder<CBBankTransfer>.BuildAllProperties(), branchID).ToList();
        }

        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var cbbanktransfer = (CBBankTransfer)(object)item;

            var connection = db.CreateConnection();
            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVECBBANKTRANSFER);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, cbbanktransfer.DocumentNo==null? "":cbbanktransfer.DocumentNo);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, cbbanktransfer.BranchID);
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, cbbanktransfer.DocumentDate);
                db.AddInParameter(savecommand, "ReferenceNo", System.Data.DbType.String, cbbanktransfer.ReferenceNo==null?"":cbbanktransfer.ReferenceNo);
                db.AddInParameter(savecommand, "ChequeNo", System.Data.DbType.String, cbbanktransfer.ChequeNo==null? "":cbbanktransfer.ChequeNo);
                db.AddInParameter(savecommand, "ChequeDate", System.Data.DbType.DateTime, cbbanktransfer.ChequeDate);
                db.AddInParameter(savecommand, "FromBankCode", System.Data.DbType.String, cbbanktransfer.FromBankCode);
                db.AddInParameter(savecommand, "FromBankAccount", System.Data.DbType.String, cbbanktransfer.FromBankAccount);
                db.AddInParameter(savecommand, "ToBankCode", System.Data.DbType.String, cbbanktransfer.ToBankCode);
                db.AddInParameter(savecommand, "ToBankAccount", System.Data.DbType.String, cbbanktransfer.ToBankAccount);
                db.AddInParameter(savecommand, "BankChargesAccount", System.Data.DbType.String, cbbanktransfer.BankChargesAccount);
                db.AddInParameter(savecommand, "WithDrawAmount", System.Data.DbType.Decimal, cbbanktransfer.WithDrawAmount);
                db.AddInParameter(savecommand, "DepositAmount", System.Data.DbType.Decimal, cbbanktransfer.DepositAmount);
                db.AddInParameter(savecommand, "Remarks", System.Data.DbType.String, cbbanktransfer.Remarks==null? "":cbbanktransfer.Remarks);
                db.AddInParameter(savecommand, "Source", System.Data.DbType.String, cbbanktransfer.Source);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, cbbanktransfer.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, cbbanktransfer.ModifiedBy);
                db.AddOutParameter(savecommand, "NewDocumentNo", System.Data.DbType.String, 25);



                result = db.ExecuteNonQuery(savecommand, transaction);

                transaction.Commit();

            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            return (result > 0 ? true : false);

        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var cbbanktransfer = (CBBankTransfer)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECBBANKTRANSFER);


                db.AddInParameter(deleteCommand, "BranchID", System.Data.DbType.Int16, cbbanktransfer.BranchID);
                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, cbbanktransfer.DocumentNo);
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
            var CBBankTransfer = ((CBBankTransfer)lookupItem);

            var cbbanktransferItem = db.ExecuteSprocAccessor(DBRoutine.SELECTCBBANKTRANSFER,
                                                    MapBuilder<CBBankTransfer>.BuildAllProperties(),
                                                    CBBankTransfer.DocumentNo, CBBankTransfer.BranchID).FirstOrDefault();
            return cbbanktransferItem;
        }

        #endregion

    }
}

