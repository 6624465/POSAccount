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
    public class APInvoiceDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public APInvoiceDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<APInvoice> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTAPINVOICE, MapBuilder<APInvoice>.MapAllProperties()
                                                    .DoNotMap(d => d.APInvoiceDetails)
                                                    .DoNotMap(d => d.AccountCode)
                                                    .Build()).ToList();
        }

        public List<APInvoice> GetListByDocumentNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTAPINVOICE, 
                                                    MapBuilder<APInvoice>
                                                    .MapAllProperties()
                                                    .DoNotMap(d => d.APInvoiceDetails)
                                                    .Build(), documentNo).ToList();
        }

        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }




        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var apinvoice = (APInvoice)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEAPINVOICE);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, apinvoice.DocumentNo);
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, apinvoice.DocumentDate);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, apinvoice.BranchID);
                db.AddInParameter(savecommand, "ReferenceNo", System.Data.DbType.String, apinvoice.ReferenceNo==null? "":apinvoice.ReferenceNo);
                db.AddInParameter(savecommand, "CreditorCode", System.Data.DbType.String, apinvoice.CreditorCode);
                db.AddInParameter(savecommand, "CreditTerm", System.Data.DbType.String, apinvoice.CreditTerm==null?"0":apinvoice.CreditTerm);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, apinvoice.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, apinvoice.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, apinvoice.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, apinvoice.LocalAmount);
                db.AddInParameter(savecommand, "DiscountAmount", System.Data.DbType.Decimal, apinvoice.DiscountAmount);
                db.AddInParameter(savecommand, "PaymentAmount", System.Data.DbType.Decimal, apinvoice.PaymentAmount);
                db.AddInParameter(savecommand, "IsVAT", System.Data.DbType.Boolean, apinvoice.IsVAT);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, apinvoice.TaxAmount);
                db.AddInParameter(savecommand, "IsWHTax", System.Data.DbType.Boolean, apinvoice.IsWHTax);
                db.AddInParameter(savecommand, "WHPercent", System.Data.DbType.Int16, 0);
                db.AddInParameter(savecommand, "WHAmount", System.Data.DbType.Decimal, apinvoice.WHAmount);
                db.AddInParameter(savecommand, "TotalAmount", System.Data.DbType.Decimal, apinvoice.TotalAmount);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, apinvoice.Remark==null?"":apinvoice.Remark);
                db.AddInParameter(savecommand, "Source", System.Data.DbType.String, apinvoice.Source);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, apinvoice.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, apinvoice.ModifiedBy);
                db.AddOutParameter(savecommand, "NewDocumentNo", System.Data.DbType.String, 25);




                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    var apDetailsItemDAL = new APInvoiceDetailDAL();
                    var glTransactionDAL = new GLTransactionDAL();
                    // Get the New Quotation No.
                    apinvoice.DocumentNo = savecommand.Parameters["@NewDocumentNo"].Value.ToString();
                    short itr = 1;
                    apinvoice.APInvoiceDetails.Where(dt=> dt.Status==true).ToList().ForEach(dt => {
                            dt.DocumentNo = apinvoice.DocumentNo;
                            dt.CreatedBy = apinvoice.CreatedBy;
                            dt.ModifiedBy = apinvoice.ModifiedBy;
                            dt.Discount = 0;
                            dt.DiscountType = "";
                            dt.ChargeCode = "";
                            dt.ItemNo = itr++;
                    });
                    itr = 1;

                    var firstDayOfMonth = new DateTime(apinvoice.DocumentDate.Year, apinvoice.DocumentDate.Month, 1);

                    apinvoice.GLTransactionDetails.Where(dt => dt.Status == true).ToList().ForEach(dt =>
                    {
                        dt.DocumentNo = apinvoice.DocumentNo;
                        dt.Source = apinvoice.Source;
                        dt.BankInSlipNo = "";
                        dt.BankStatementPgNo = 0;
                        dt.BankStatementTotalPgNo = 0;
                        dt.ChequeNo = "";
                        dt.BranchID = apinvoice.BranchID;
                        dt.DocumentDate = apinvoice.DocumentDate;
                        dt.DocumentType = "INV";
                        dt.DebtorCode = "";
                        dt.CreditorCode = apinvoice.CreditorCode;
                        dt.CurrencyCode = "THB";
                        dt.AccountDate = firstDayOfMonth;
                        dt.ItemNo = itr++;
                    });



                    result = Convert.ToInt16(apDetailsItemDAL.Delete(apinvoice.DocumentNo, transaction));
                    result = apDetailsItemDAL.SaveList(apinvoice.APInvoiceDetails, transaction) == true ? 1 : 0;


                    
                    result = glTransactionDAL.SaveList(apinvoice.GLTransactionDetails, transaction) == true ? 1 : 0;

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

                throw ex;
            }

            return (result > 0 ? true : false);

        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var apinvoice = (APInvoice)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEAPINVOICE);


                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, apinvoice.DocumentNo);
                db.AddInParameter(deleteCommand, "CancelledBy", System.Data.DbType.String, apinvoice.CancelledBy);

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
            var item = ((APInvoice)lookupItem);

            var apinvoiceItem = db.ExecuteSprocAccessor(DBRoutine.SELECTAPINVOICE,
                                                    MapBuilder<APInvoice>.MapAllProperties()
                                                    .DoNotMap(a=> a.AccountCode)
                                                    .DoNotMap(d => d.APInvoiceDetails)
                                                    .Build(),
                                                    item.DocumentNo).FirstOrDefault();

            if (apinvoiceItem == null)
                return null;


            if (apinvoiceItem != null)
            {
                apinvoiceItem.APInvoiceDetails = new POSAccount.DataFactory.APInvoiceDetailDAL().GetListByDocumentNo(apinvoiceItem.DocumentNo);
            }
            
            
            return apinvoiceItem;
        }

        #endregion





        public System.Data.IDataReader PerformSearch(string whereClause)
        {
            var searchcommand = db.GetStoredProcCommand(DBRoutine.SEARCHAPINVOICE);
            db.AddInParameter(searchcommand, "whereClause", System.Data.DbType.String, whereClause);
            return db.ExecuteReader(searchcommand);
        }



    }
}

