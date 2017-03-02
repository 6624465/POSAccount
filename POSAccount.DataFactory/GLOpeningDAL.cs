using Microsoft.Practices.EnterpriseLibrary.Data;
using POSAccount.Contract;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace POSAccount.DataFactory
{
    public class GLOpeningDAL  
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GLOpeningDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<GLOpening> GetList(Int32 financialYear, short branchID)
        {
            return db.ExecuteSprocAccessor(DBRoutine.SELECTGLOPENING, MapBuilder<GLOpening>
                                                                            .MapAllProperties()
                                                                            .DoNotMap(d => d.GLOpenDetails)
                                                                            .DoNotMap(d => d.GLTransactionDetails)
                                                                            .DoNotMap(d => d.AccountCode)
                                                                            .DoNotMap(d => d.DebitCredit)
                                                                            .DoNotMap(d => d.DetailRemark)
                                                                            .Build(),financialYear,branchID).ToList();
        }


        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }

        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var glopening = (GLOpening)(object)item;

            var connection = db.CreateConnection();
            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEGLOPENING);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, glopening.DocumentNo);
                db.AddInParameter(savecommand, "FinancialYear", System.Data.DbType.Int32, glopening.FinancialYear);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, glopening.BranchID);
                db.AddInParameter(savecommand, "AccountDate", System.Data.DbType.DateTime, glopening.AccountDate);
                db.AddInParameter(savecommand, "TotalDebitAmount", System.Data.DbType.Decimal, glopening.TotalDebitAmount);
                db.AddInParameter(savecommand, "TotalCreditAmount", System.Data.DbType.Decimal, glopening.TotalCreditAmount);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, glopening.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, glopening.ModifiedBy);
                db.AddOutParameter(savecommand, "NewDocumentNo", System.Data.DbType.String, 25);



                result = db.ExecuteNonQuery(savecommand, transaction);
                if (result > 0)
                {
                    var glopeningdetailsDAL = new GLOpeningDetailDAL();
                    var glTransactionDAL = new GLTransactionDAL();

                    
                    // Get the New Quotation No.
                    glopening.DocumentNo = savecommand.Parameters["@NewDocumentNo"].Value.ToString();
                    short itr = 1;
                    glopening.GLOpenDetails.ForEach(dt =>
                    {
                        dt.DocumentNo = glopening.DocumentNo;
                        dt.Remark = "";
                    }
                        );
                    result = Convert.ToInt16(glopeningdetailsDAL.Delete(glopening.DocumentNo, transaction));
                    result = glopeningdetailsDAL.SaveList(glopening.GLOpenDetails, transaction) == true ? 1 : 0;

                    var firstDayOfMonth = new DateTime(glopening.AccountDate.Year, glopening.AccountDate.Month, 1);


                    glopening.GLOpenDetails.ForEach(dt =>{
                    
                    
                        glopening.GLTransactionDetails.Add( new GLTransaction{
                            ItemNo = itr++,
                            BranchID = glopening.BranchID,
                            AccountCode = dt.AccountCode,
                            AccountDate = firstDayOfMonth,
                            Source = "GL",
                            DocumentType = "OB",
                            DocumentNo = glopening.DocumentNo,
                            DocumentDate = glopening.AccountDate,
                            DebtorCode = "",
                            CreditorCode = "",
                            ChequeNo = "",
                            BankInSlipNo = "",
                            BankStatementPgNo = 0,
                            CurrencyCode = "THB",
                            ExchangeRate=0,
                            BaseAmount=0,
                            LocalAmount=0,
                            CreditAmount = dt.CreditAmount,
                            DebitAmount = dt.DebitAmount,
                            Remark = "",
                            BankStatementTotalPgNo = 0,
                            });
                    
                    
                    });

                    result = glTransactionDAL.SaveList(glopening.GLTransactionDetails, transaction) == true ? 1 : 0;




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
            var glopening = (GLOpening)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEGLOPENING);

                db.AddInParameter(deleteCommand, "BranchID", System.Data.DbType.Int16, glopening.BranchID);
                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, glopening.DocumentNo);
                db.AddInParameter(deleteCommand, "FinancialYear", System.Data.DbType.Int32, glopening.FinancialYear);

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
            var item = ((GLOpening)lookupItem);

            var glopeningItem = db.ExecuteSprocAccessor(DBRoutine.SELECTGLOPENING,
                                                    MapBuilder<GLOpening>.MapAllProperties()
                                                                            .DoNotMap(d => d.GLOpenDetails)
                                                                            .DoNotMap(d => d.GLTransactionDetails)
                                                                            .DoNotMap(d => d.AccountCode)
                                                                            .DoNotMap(d => d.DebitCredit)
                                                                            .DoNotMap(d => d.DetailRemark)
                                                                            .Build(), item.DocumentNo,item.FinancialYear,item.BranchID).FirstOrDefault();

            if (glopeningItem == null)
                return null;


            if (glopeningItem != null)
            {
                glopeningItem.GLOpenDetails = new POSAccount.DataFactory.GLOpeningDetailDAL().GetList(glopeningItem.DocumentNo);
            }
            
            return glopeningItem;
        }

        #endregion

    }
}

