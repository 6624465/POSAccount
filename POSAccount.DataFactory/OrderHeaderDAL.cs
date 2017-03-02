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
    public class OrderHeaderDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderHeaderDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<OrderHeader> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTORDERHEADER,
                                             MapBuilder<OrderHeader>
                                             .MapAllProperties()
                                             .DoNotMap(d => d.OrderDetails)
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

            var orderheader = (OrderHeader)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEORDERHEADER);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, orderheader.DocumentNo);
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, orderheader.DocumentDate);
                db.AddInParameter(savecommand, "CustomerCode", System.Data.DbType.String, orderheader.CustomerCode??"");
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, orderheader.BranchID);
                db.AddInParameter(savecommand, "OrderType", System.Data.DbType.String, orderheader.OrderType);
                db.AddInParameter(savecommand, "PaymentType", System.Data.DbType.String, orderheader.PaymentType);
                db.AddInParameter(savecommand, "Status", System.Data.DbType.Boolean, orderheader.Status);
                db.AddInParameter(savecommand, "TotalAmount", System.Data.DbType.Decimal, orderheader.TotalAmount);
                db.AddInParameter(savecommand, "DiscountAmount", System.Data.DbType.Decimal, orderheader.DiscountAmount);
                db.AddInParameter(savecommand, "PaymentAmount", System.Data.DbType.Decimal, orderheader.PaymentAmount);
                db.AddInParameter(savecommand, "IsVAT", System.Data.DbType.Boolean, orderheader.IsVAT);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, orderheader.TaxAmount);
                db.AddInParameter(savecommand, "IsWHTax", System.Data.DbType.Boolean, orderheader.IsWHTax);
                db.AddInParameter(savecommand, "WithHoldingAmount", System.Data.DbType.Decimal, orderheader.WithHoldingAmount);
                db.AddInParameter(savecommand, "NetAmount", System.Data.DbType.Decimal, orderheader.NetAmount);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, orderheader.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, orderheader.ModifiedBy);
                db.AddOutParameter(savecommand, "NewDocumentNo", System.Data.DbType.String, 25);



                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    var orderdetailsDAL = new OrderDetailsDAL();
                    // Get the New DocumentNo No.
                    orderheader.DocumentNo = savecommand.Parameters["@NewDocumentNo"].Value.ToString();
                    short itr = 1;
                    orderheader.OrderDetails.ForEach(dt =>
                    {
                        dt.ItemNo = itr++;
                        dt.DocumentNo = orderheader.DocumentNo;
                        dt.CreatedBy = orderheader.CreatedBy;
                        dt.ModifiedBy = orderheader.ModifiedBy;
                        dt.Status = true;
                        dt.DiscountType =dt.DiscountType == null ? "" : dt.DiscountType;
                        dt.SellPrice = decimal.Multiply(dt.SellRate, dt.Quantity);

                    }
                        );
                    result = Convert.ToInt16(orderdetailsDAL.Delete(orderheader.DocumentNo, transaction));
                    result = orderdetailsDAL.SaveList(orderheader.OrderDetails, transaction) == true ? 1 : 0;

                    #region GENERATE AR INVOICE




                     

                    //-------------------------------------- ***** GENERATE AR-INVOICE ***** --------------------------------------------------------//

                    if (result>0)
                    {
                        var arHeader = new POSAccount.Contract.ARInvoice();
                        List<POSAccount.Contract.ARInvoiceDetail> lstARInvoiceItems = new List<POSAccount.Contract.ARInvoiceDetail>();
                        List<POSAccount.Contract.GLTransaction> lsgGLtransactionItems = new List<POSAccount.Contract.GLTransaction>();

                        foreach (var dt in orderheader.OrderDetails)
                        {
                            POSAccount.Contract.ChargeCodeMaster chargeCodeItem =(POSAccount.Contract.ChargeCodeMaster) new POSAccount.DataFactory.ChargeCodeDAL().GetItem<POSAccount.Contract.ChargeCodeMaster>(new POSAccount.Contract.ChargeCodeMaster{ChargeCode=dt.ChargeCode});

                            var mappingAccountCode = "";
                            var creditdebit = "";

                            if(chargeCodeItem!=null)
	                        {
                                mappingAccountCode = chargeCodeItem.AccountCode;

                                if (mappingAccountCode.Length>0)
                                {
                                    creditdebit = new POSAccount.DataFactory.ChartOfAccountDAL().GetList(orderheader.BranchID).Where(x => x.AccountCode == mappingAccountCode).FirstOrDefault().DebitCredit;
                                }
                                
	                        }

                            /*TODO:
                             
                            1. if the order hearder got the WHtax and VAT ticked, then
                                automatically add the 2 rows to the GLTransaction table (list) below. 
                             * 
                             
                             */



                            lstARInvoiceItems.Add(new ARInvoiceDetail
                            {
                                DocumentNo = "",
                                AccountCode=mappingAccountCode,
                                AccountCodeDescription = "",
                                BaseAmount = dt.SellPrice,
                                ChargeCode=dt.ChargeCode,
                                ChargeCodeDescription = dt.ChargeDescription,
                                CurrencyCode="THB",
                                Discount = dt.Discount,
                                DiscountType = dt.DiscountType,
                                
                                ExchangeRate = Convert.ToDecimal(0.00),
                                ItemNo = dt.ItemNo,
                                LocalAmount = dt.SellPrice,
                                LocalAmountWithTax = dt.SellPrice,
                                OrderNo = dt.DocumentNo,
                                Remark=orderheader.DocumentNo,
                                TaxAmount = Convert.ToDecimal(0.00),
                                TaxCode="",
                                CreatedBy=dt.CreatedBy,
                                ModifiedBy = dt.ModifiedBy,
                                
                            });

                            lsgGLtransactionItems.Add(new GLTransaction
                            {
                                DocumentNo = "",
                                AccountCode = mappingAccountCode,
                                BaseAmount = dt.SellPrice,
                                CurrencyCode = "THB",
                                ExchangeRate = Convert.ToDecimal(0.00),
                                ItemNo = dt.ItemNo,
                                LocalAmount = dt.SellPrice,
                                Remark = "",
                                Status = true,
                                Source="AR",
                                CreditorCode =orderheader.CustomerCode,
                                DocumentDate = orderheader.DocumentDate,
                                BankInSlipNo="",
                                AccountDate = orderheader.DocumentDate,
                                DocumentType="REC",
                                BranchID = orderheader.BranchID,
                                Amount = orderheader.NetAmount,
                                CreditAmount = orderheader.NetAmount,
                                DebitAmount =0,
                                BankStatementPgNo=0,
                                BankStatementTotalPgNo=0,
                                ChequeNo="",
                                DebitCredit = creditdebit,
                                DetailRemark="",
                                Period=0,
                            });
                        }

                        arHeader.DocumentNo = "";
                        arHeader.DocumentDate = orderheader.DocumentDate;
                        arHeader.CreatedBy = orderheader.CreatedBy;
                        arHeader.ModifiedBy = orderheader.ModifiedBy;
                        arHeader.DebtorCode= orderheader.CustomerCode;
                        arHeader.BranchID = orderheader.BranchID;
                        arHeader.CreditTerm = "";
                        arHeader.CurrencyCode = "THB";
                        arHeader.ExchangeRate = Convert.ToDecimal(0.00);
                        arHeader.BaseAmount= orderheader.TotalAmount;
                        arHeader.TaxAmount = 0;
                        arHeader.LocalAmount = orderheader.TotalAmount;
                        arHeader.NetAmount = orderheader.NetAmount;
                        arHeader.WHPercent = Convert.ToInt16(3);
                        arHeader.IsWHTax = orderheader.IsWHTax;
                        arHeader.IsVAT = orderheader.IsVAT;
                        arHeader.WithHoldingAmount = orderheader.WithHoldingAmount;
                        arHeader.TaxAmount = orderheader.TaxAmount;
                        arHeader.DiscountAmount = orderheader.DiscountAmount;
                        arHeader.IsCancel=false;
                        arHeader.ReferenceNo = orderheader.DocumentNo;
                        arHeader.Remark="";
                        arHeader.Source = "ARINV";
                         


                        arHeader.ARInvoiceDetails = lstARInvoiceItems;

                        arHeader.GLTransactionDetails = lsgGLtransactionItems;


                        result = new POSAccount.DataFactory.ARInvoiceDAL().Save(arHeader, transaction) == true ? 1 : 0;
                    
 

                    }

                        //-------------------------------------- ***** END GENERATE AR-INVOICE ******--------------------------------------------------------//
                     
                     

                    #endregion
                
                
                
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
            var orderheader = (OrderHeader)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEORDERHEADER);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, orderheader.DocumentNo);
                db.AddInParameter(deleteCommand, "CancelledBy", System.Data.DbType.String, orderheader.CreatedBy);

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
            var item = ((OrderHeader)lookupItem);

            var orderheaderItem = db.ExecuteSprocAccessor(DBRoutine.SELECTORDERHEADER,
                                                     MapBuilder<OrderHeader>
                                                     .MapAllProperties()
                                                     .DoNotMap(d => d.OrderDetails)
                                                     .Build(),
                                                    item.DocumentNo).FirstOrDefault();

            if (orderheaderItem!=null)
            {
                orderheaderItem.OrderDetails = new POSAccount.DataFactory.OrderDetailsDAL().GetListByQuotationNo(orderheaderItem.DocumentNo).ToList();
            }

            return orderheaderItem;
        }

        #endregion


        public System.Data.IDataReader PerformSearch(string whereClause)
        {
            var searchcommand = db.GetStoredProcCommand(DBRoutine.SEARCHORDER);
            db.AddInParameter(searchcommand, "whereClause", System.Data.DbType.String, whereClause);
            return db.ExecuteReader(searchcommand);
        }



    }
}

