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

    public class GLJournalDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GLJournalDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<GLJournal> GetList(short branchID)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTGLJOURNAL, MapBuilder<GLJournal>
                                                                    .MapAllProperties()
                                                                    .DoNotMap(d => d.GLJournalDetails)
                                                                    .DoNotMap(d => d.GLTransactionDetails)
                                                                    .DoNotMap(d => d.AccountCode)
                                                                    .DoNotMap(d => d.DebitCredit)
                                                                    .DoNotMap(d => d.DetailRemark)
                                                                    .Build(),
                                                                    branchID
                                                                    ).ToList();
        }

        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }




        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var gljournal = (GLJournal)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try

            {

                var lstActiveGLDetails = gljournal.GLJournalDetails.Where(x => x.Status == true).ToList();

                gljournal.TotalCreditAmount = lstActiveGLDetails.Sum(x => x.BaseCreditAmount);
                gljournal.TotalDebitAmount = lstActiveGLDetails.Sum(x => x.BaseDebitAmount);
                
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEGLJOURNAL);

                db.AddInParameter(savecommand, "Source", System.Data.DbType.String, gljournal.Source);
                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, gljournal.DocumentNo);
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, gljournal.DocumentDate);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, gljournal.BranchID);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, gljournal.Remark);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, gljournal.CurrencyCode==null? "":gljournal.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, gljournal.ExchangeRate);
                db.AddInParameter(savecommand, "TotalDebitAmount", System.Data.DbType.Decimal, gljournal.TotalDebitAmount);
                db.AddInParameter(savecommand, "TotalCreditAmount", System.Data.DbType.Decimal, gljournal.TotalCreditAmount);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, gljournal.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, gljournal.ModifiedBy);
                db.AddOutParameter(savecommand, "NewDocumentNo", System.Data.DbType.String, 25);
                

                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    var GLJournalDetailsItemDAL = new GLJournalDetailDAL();
                    var glTransactionDAL = new GLTransactionDAL();
                    
                    
                    // Get the New Quotation No.
                    gljournal.DocumentNo = savecommand.Parameters["@NewDocumentNo"].Value.ToString();
                    short itr = 1;
                    gljournal.GLJournalDetails.ForEach(dt =>
                    {
                        dt.DocumentNo = gljournal.DocumentNo;
                        dt.ItemNo = itr++;
                    }
                        );
                    result = Convert.ToInt16(GLJournalDetailsItemDAL.Delete(gljournal.DocumentNo, transaction));

                    

                    result = GLJournalDetailsItemDAL.SaveList(lstActiveGLDetails, transaction) == true ? 1 : 0;


                    var firstDayOfMonth = new DateTime(gljournal.DocumentDate.Year, gljournal.DocumentDate.Month, 1);

                    gljournal.GLTransactionDetails = new List<GLTransaction>();

                    itr = 1;
                    lstActiveGLDetails.ForEach(dt =>
                    {
                        gljournal.GLTransactionDetails.Add(new GLTransaction
                        {
                            ItemNo = itr++,
                            BranchID = gljournal.BranchID,
                            AccountCode = dt.AccountCode,
                            AccountDate = firstDayOfMonth,
                            Source = "GL",
                            DocumentType = "JV",
                            DocumentNo = gljournal.DocumentNo,
                            DocumentDate = DateTime.Now.Date,
                            DebtorCode = "",
                            CreditorCode = "",
                            ChequeNo = "",
                            BankInSlipNo = "",
                            BankStatementPgNo = 0,
                            CurrencyCode = "THB",
                            ExchangeRate = 0,
                            BaseAmount = 0,
                            LocalAmount = 0,
                            CreditAmount = dt.BaseCreditAmount,
                            DebitAmount = dt.BaseDebitAmount,
                            Remark = "",
                            BankStatementTotalPgNo = 0,
                        });


                    });


                    result = glTransactionDAL.Delete(gljournal.DocumentNo, gljournal.BranchID, transaction) == true ? 1 : 0;

                    result = glTransactionDAL.SaveList(gljournal.GLTransactionDetails, transaction) == true ? 1 : 0;

                }

                if (result > 0)
                    transaction.Commit();
                else
                    transaction.Rollback();

            }
            catch (Exception ex)
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
            var gljournal = (GLJournal)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEGLJOURNAL);

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


        public bool Delete(string documentNo, short branchID)
        {
            var result = false;


            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEGLJOURNAL);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, documentNo);
                db.AddInParameter(deleteCommand, "BranchID", System.Data.DbType.Int16, branchID);

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
            var item = ((GLJournal)lookupItem);

            var GLJournalItem = db.ExecuteSprocAccessor(DBRoutine.SELECTGLJOURNAL,
                                                    MapBuilder<GLJournal>
                                                        .MapAllProperties()
                                                        .DoNotMap(d => d.GLJournalDetails)
                                                        .DoNotMap(d => d.GLTransactionDetails)
                                                        .DoNotMap(d => d.AccountCode)
                                                        .DoNotMap(d => d.DebitCredit)
                                                        .DoNotMap(d => d.DetailRemark)
                                                        .Build(),
                                                    item.DocumentNo,item.BranchID).FirstOrDefault();


            if (GLJournalItem == null)
                return null;


            if (GLJournalItem != null)
            {
                GLJournalItem.GLJournalDetails = new POSAccount.DataFactory.GLJournalDetailDAL().GetListByDocumentNo(GLJournalItem.DocumentNo);
            }


            return GLJournalItem;
        }

        #endregion




        public System.Data.IDataReader PerformSearch(string whereClause)
        {
            var searchcommand = db.GetStoredProcCommand(DBRoutine.SEARCHGL);
            db.AddInParameter(searchcommand, "whereClause", System.Data.DbType.String, whereClause);
            return db.ExecuteReader(searchcommand);
        }




    }
}
