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
    public class ARInvoiceDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ARInvoiceDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<ARInvoice> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTARINVOICE,
                                            MapBuilder<ARInvoice>
                                            .MapAllProperties()
                                            .DoNotMap(d => d.ARInvoiceDetails)
                                            .DoNotMap(d => d.AccountCode)
                                            .Build()).ToList();
        }

        public List<ARInvoice> GetListByDocumentNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTARINVOICE,
                                                    MapBuilder<ARInvoice>
                                                    .MapAllProperties()
                                                    .DoNotMap(d => d.ARInvoiceDetails)
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

            var arinvoice = (ARInvoice)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEARINVOICE);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, arinvoice.DocumentNo);
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, arinvoice.DocumentDate);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, arinvoice.BranchID);
                db.AddInParameter(savecommand, "ReferenceNo", System.Data.DbType.String, arinvoice.ReferenceNo ?? "");
                db.AddInParameter(savecommand, "DebtorCode", System.Data.DbType.String, arinvoice.DebtorCode);
                db.AddInParameter(savecommand, "CreditTerm", System.Data.DbType.String, arinvoice.CreditTerm ?? "");
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, arinvoice.CurrencyCode ?? "THB");
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, arinvoice.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, arinvoice.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, arinvoice.LocalAmount);
                db.AddInParameter(savecommand, "DiscountAmount", System.Data.DbType.Decimal, arinvoice.DiscountAmount);
                db.AddInParameter(savecommand, "PaymentAmount", System.Data.DbType.Decimal, arinvoice.PaymentAmount);
                db.AddInParameter(savecommand, "IsVAT", System.Data.DbType.Boolean, arinvoice.IsVAT);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, arinvoice.TaxAmount);
                db.AddInParameter(savecommand, "IsWHTax", System.Data.DbType.Boolean, arinvoice.IsWHTax);
                db.AddInParameter(savecommand, "WHPercent", System.Data.DbType.Int16, arinvoice.WHPercent);
                db.AddInParameter(savecommand, "WithHoldingAmount", System.Data.DbType.Decimal, arinvoice.WithHoldingAmount);
                db.AddInParameter(savecommand, "NetAmount", System.Data.DbType.Decimal, arinvoice.NetAmount);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, arinvoice.Remark ?? "");
                db.AddInParameter(savecommand, "Source", System.Data.DbType.String, arinvoice.Source ?? "");
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, arinvoice.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, arinvoice.ModifiedBy);
                db.AddInParameter(savecommand, "PrintRemarks", System.Data.DbType.String, arinvoice.PrintRemarks ?? "");
                db.AddOutParameter(savecommand, "NewDocumentNo", System.Data.DbType.String, 25);




                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    var arDetailsItemDAL = new ARInvoiceDetailDAL();
                    var glTransactionDAL = new GLTransactionDAL();
                    // Get the New Quotation No.
                    arinvoice.DocumentNo = savecommand.Parameters["@NewDocumentNo"].Value.ToString();

                    short itr = 1;

                    arinvoice.ARInvoiceDetails.ForEach(dt =>
                        {
                            dt.DocumentNo = arinvoice.DocumentNo;
                            dt.CreatedBy = arinvoice.CreatedBy;
                            dt.ModifiedBy = arinvoice.ModifiedBy;
                            dt.Discount=0;
                            dt.DiscountType="";
                            
                            dt.ItemNo = itr++;
                        });

                    

                   
                    itr = 1;

                    var firstDayOfMonth = new DateTime(arinvoice.DocumentDate.Year, arinvoice.DocumentDate.Month, 1);

                    arinvoice.GLTransactionDetails.Where(dt => dt.Status == true).ToList().ForEach(dt =>
                    {
                        dt.DocumentNo = arinvoice.DocumentNo;
                        dt.Source = arinvoice.Source;
                        dt.BankInSlipNo = "";
                        dt.BankStatementPgNo = 0;
                        dt.BankStatementTotalPgNo = 0;
                        dt.ChequeNo = "";
                        dt.BranchID = arinvoice.BranchID;
                        dt.DocumentDate = arinvoice.DocumentDate;
                        dt.DocumentType = "INV";
                        dt.DebtorCode = arinvoice.DebtorCode;
                        dt.CreditorCode = "";
                        dt.CurrencyCode = "THB";
                        dt.AccountDate = firstDayOfMonth;

                        dt.ItemNo = itr++;
                    });



                    result = Convert.ToInt16(arDetailsItemDAL.Delete(arinvoice.DocumentNo, transaction));
                    result = arDetailsItemDAL.SaveList(arinvoice.ARInvoiceDetails, transaction) == true ? 1 : 0;

                    result = glTransactionDAL.SaveList(arinvoice.GLTransactionDetails, transaction) == true ? 1 : 0;

                }

                if (result > 0)
                    if (currentTransaction == null)
                        transaction.Commit();
                    else
                        if (currentTransaction == null)
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
            var arinvoice = (ARInvoice)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEARINVOICE);


                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, arinvoice.DocumentNo);
                db.AddInParameter(deleteCommand, "CancelledBy", System.Data.DbType.String, arinvoice.CancelledBy);
                db.AddInParameter(deleteCommand, "CancelReason", System.Data.DbType.String, arinvoice.CancelReason);


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
            var item = ((ARInvoice)lookupItem);

            var arinvoiceItem = db.ExecuteSprocAccessor(DBRoutine.SELECTARINVOICE,
                                                    MapBuilder<ARInvoice>
                                                    .MapAllProperties()
                                                    .DoNotMap(d => d.ARInvoiceDetails)
                                                    .DoNotMap(d => d.AccountCode)
                                                    .Build(),
                                                    item.DocumentNo).FirstOrDefault();

            if (arinvoiceItem == null)
                return null;


            if (arinvoiceItem != null)
            {
                arinvoiceItem.ARInvoiceDetails = new POSAccount.DataFactory.ARInvoiceDetailDAL().GetListByDocumentNo(arinvoiceItem.DocumentNo);

                arinvoiceItem.GLTransactionDetails = new POSAccount.DataFactory.GLTransactionDAL().GetList(arinvoiceItem.DocumentNo);
            }

            return arinvoiceItem;
        }

        #endregion



        public System.Data.IDataReader PerformSearch(string whereClause)
        {
            var searchcommand = db.GetStoredProcCommand(DBRoutine.SEARCHARINVOICE);
            db.AddInParameter(searchcommand, "whereClause", System.Data.DbType.String, whereClause);
            return db.ExecuteReader(searchcommand);
        }





    }
}

