using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSAccount.Contract;

namespace POSAccount.Areas.POS.Controllers
{
    [RouteArea("POS")]
    [Authorize]
    public class POSController : Controller
    {
        // GET: POS/POS
        public ActionResult Index()
        {
            return View();
        }

        [Route("SalesOrder")]
        public ActionResult SalesOrder(string documentNo)
        {
            POSAccount.Contract.OrderHeader orderHeader = null;

            if (documentNo == "NEW" || documentNo == null)
            {
                orderHeader = new POSAccount.Contract.OrderHeader();
                orderHeader.OrderDetails = new List<OrderDetails>();
            }
            else {

                orderHeader = new POSAccount.BusinessFactory.OrderHeaderBO().GetOrderHeader(new OrderHeader { DocumentNo = documentNo });

                if (orderHeader.OrderDetails==null)
                {
                    orderHeader.OrderDetails = new List<OrderDetails>();
                }



            }


            IEnumerable<SelectListItem> lstChargeCode = Utility.GetChargeCodeItemList();
            IEnumerable<SelectListItem> lstDiscount= Utility.GetDiscountList();


            foreach (var item in orderHeader.OrderDetails)
            {
                item.ChargeCodeList = lstChargeCode;
                item.DiscountTypeList = lstDiscount;
            }


            orderHeader.DocumentDate = DateTime.Today.Date;
            orderHeader.CustomerList = Utility.GetDebtorList();
            orderHeader.OrderTypeList = Utility.GetLookupItemList("OrderType");
            orderHeader.PaymentTypeList = Utility.GetLookupItemList("PaymentTerm");

            

            return View("SalesOrder",orderHeader);
        }

        [Route("AddOrderItem")]
        [HttpGet]
        public ActionResult AddOrderItem(string documentNo, Int16 itemNo,string customerCode,string orderType)
        {
            OrderDetails orderdetailsItem= null;

            if (documentNo=="NEW")
            {
                documentNo = string.Empty;
            }


            if (documentNo == string.Empty || documentNo == null )
            {
                orderdetailsItem = new OrderDetails();
            }
            else
            {
                orderdetailsItem = new POSAccount.BusinessFactory.OrderHeaderBO().GetOrderHeader(new Contract.OrderHeader{ DocumentNo = documentNo })
                       .OrderDetails.Where(dt => dt.ItemNo == itemNo).FirstOrDefault();
                if (orderdetailsItem == null)
                {
                    orderdetailsItem = new OrderDetails();
                }
            }
            orderdetailsItem.ChargeCodeList = Utility.GetChargeCodeItemList();
            orderdetailsItem.JoiningDate = DateTime.Now.Date;
            ViewBag.CustomerCode = customerCode;
            ViewBag.OrderType = orderType;
            
            return PartialView("AddOrderItem", orderdetailsItem);
            //return PartialView("AddQuotationItem");
        }

        [Route("DeleteOrderHeader")]
        [HttpPost]
        public JsonResult DeleteOrderHeader(POSAccount.Contract.OrderHeader OrderHeader)
        {
            try
            {
                OrderHeader.CreatedBy = Utility.DEFAULTUSER;
                var result = new POSAccount.BusinessFactory.OrderHeaderBO().DeleteOrderHeader(OrderHeader);
                if (result)
                    return Json(new { success = true, Message = "Record deleted successfully.", orderData = OrderHeader });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Message = ex.Message, orderData = OrderHeader });
            }
            return Json(new { success = true, Message = "Record deleted successfully.", orderData = OrderHeader });
        }

        [Route("SaveOrderHeader")]
        [HttpPost]
        public JsonResult SaveOrderHeader(POSAccount.Contract.OrderHeader OrderHeader)
        {
            try
            {
                OrderHeader.CreatedBy = Utility.DEFAULTUSER;
                OrderHeader.ModifiedBy = Utility.DEFAULTUSER;
                OrderHeader.Status = Utility.DEFAULTSTATUS;
                OrderHeader.BranchID = Utility.SsnBranch;

                if (OrderHeader.OrderDetails.Count == 0 || OrderHeader.OrderDetails.Count == null)
                {
                    //Quotationdata.CompanyAddress.AddressType = "Company";
                    //Quotationdata.CompanyAddress.SeqNo = 1;
                    //Quotationdata.CompanyAddress.AddressLinkID = company.CompanyCode;

                }

                var result = new POSAccount.BusinessFactory.OrderHeaderBO().SaveOrderHeader(OrderHeader);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }


            return Json(new { success = true, Message = "Order saved successfully.", orderData = OrderHeader });

        }



        [Route("GetChargeCodePrice")]
        public JsonResult GetChargeCodePrice(string customerCode,string orderType, string chargeCode)
        {
            var orderdetail = new POSAccount.Contract.OrderDetails();

            orderdetail.Salary = 0;
            orderdetail.SellPrice = 0;
            orderdetail.SellRate = 0;

            if (orderType=="NON-RECRUIT")
            {
                orderdetail = new POSAccount.BusinessFactory.OrderDetailsBO().GetChargeCodePrice(customerCode, chargeCode);
                
            }

            return Json(new { OrderDetailsData = orderdetail }, JsonRequestBehavior.AllowGet);

        }



        [Route("Orders")]
        [HttpGet]
        public ActionResult Orders()
        {
            var lstOrders = new POSAccount.BusinessFactory.OrderHeaderBO().GetList().Where(ord => ord.Status == true).ToList();
            //HttpContext.Session["OrderItems"] = null;
            return View("Orders", lstOrders);


        }

    }
}