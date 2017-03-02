using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POSAccount.Areas.User.Controllers
{
    [RouteArea("User")]
    [SessionFilter]
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region Users
        [Route("UserList")]
        [HttpGet]
        public ActionResult UserList()
        {
            var lstUsers = new POSAccount.BusinessFactory.UsersBO().GetList();
            return View("UserList", lstUsers);
        }

        [Route("EditUser")]
        [HttpGet]
        public ActionResult EditUser(string userID)
        {
            var user = new POSAccount.Contract.Users();

            if (userID == "NEW")
            {
                userID = "";
                user = new Contract.Users();
            }


            if (userID != null && userID.Length > 0)
                user = new POSAccount.BusinessFactory.UsersBO().GetUsers(new Contract.Users { UserID = userID });


            user.RoleCodeList = Utility.GetRoleList();

            return View("UserProfile", user);
        }


        [Route("SaveUser")]
        [HttpPost]
        public JsonResult SaveUser(POSAccount.Contract.Users user)
        {
            try
            {

                user.LogInStatus = true;
                user.CreatedBy = Utility.DEFAULTUSER;
                user.ModifiedBy = Utility.DEFAULTUSER;
                var result = new POSAccount.BusinessFactory.UsersBO().SaveUsers(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }
            return Json(new { success = true, Message = "USER PROFILE saved successfully.", userData = user });
        }

        [Route("RoleRights")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult RoleRights(string Role = "")
        {
            List<POSAccount.ViewModals.Search.LayoutMenuRights> lstMenu = new List<POSAccount.ViewModals.Search.LayoutMenuRights>();
            if(!string.IsNullOrWhiteSpace(Role))
            {
                var lstUsers = new POSAccount.BusinessFactory.UsersBO().GetList();
                var roleRights = new POSAccount.BusinessFactory.RoleRightsBO()
                                    .GetList(Role);

                var securablesAll = (List<POSAccount.Contract.Securables>)System.Web.HttpContext.Current.Application["AppSecurables"];

                var securables = securablesAll.Join(roleRights,
                                    sec => sec.SecurableItem,
                                    rig => rig.SecurableItem,
                                    (sec, rig) => new { a = sec, b = rig })
                                .Select(x => new POSAccount.Contract.Securables()
                                {
                                    SecurableItem = x.a.SecurableItem,
                                    GroupID = x.a.GroupID,
                                    Description = x.a.Description,
                                    ActionType = x.a.ActionType,
                                    Link = x.a.Link,
                                    Icon = x.a.Icon,
                                    Sequence = x.a.Sequence,
                                    ParentSequence = x.a.ParentSequence
                                })
                                .ToList<POSAccount.Contract.Securables>();


                var menuItems = securablesAll.Where(x => x.ActionType == "TopMenu")
                                    .Select(x => new { securableItem = x.SecurableItem, Icon = x.Icon, GroupId = x.GroupID }).Distinct().ToList();


                for (var i = 0; i < menuItems.Count; i++)
                {
                    POSAccount.ViewModals.Search.LayoutMenuRights item = new POSAccount.ViewModals.Search.LayoutMenuRights();
                    item.MenuName = menuItems[i].securableItem;
                    item.Icon = menuItems[i].Icon;
                    item.securablesLst = securablesAll.Where(x => x.GroupID == menuItems[i].securableItem && (x.ActionType == "Menu"))
                                                   .Select(x => new POSAccount.ViewModals.Search.SecurablesRights
                                                   {
                                                       SecurableItem = x.SecurableItem,
                                                       GroupID = x.GroupID,
                                                       Description = x.Description,
                                                       ActionType = x.ActionType,
                                                       Link = x.Link,
                                                       Icon = x.Icon,
                                                       hasRight = (securables.Where(j => j.SecurableItem == x.SecurableItem).Count() > 0),
                                                       Sequence = x.Sequence,
                                                       ParentSequence = x.ParentSequence,
                                                       ActionMenus = securablesAll.Where(y => y.GroupID == menuItems[i].securableItem && (y.ActionType == "Action") && y.ParentSequence == x.Sequence)
                                                                                   .Select(y => new POSAccount.ViewModals.Search.SecurablesRights
                                                                                   {
                                                                                       SecurableItem = y.SecurableItem,
                                                                                       GroupID = y.GroupID,
                                                                                       Description = y.Description,
                                                                                       ActionType = y.ActionType,
                                                                                       Link = y.Link,
                                                                                       Icon = y.Icon,
                                                                                       hasRight = (securables.Where(jk => jk.SecurableItem == y.SecurableItem).Count() > 0),
                                                                                       Sequence = y.Sequence,
                                                                                       ParentSequence = y.ParentSequence
                                                                                   }).ToList<POSAccount.ViewModals.Search.SecurablesRights>()
                                                   }).OrderBy(x => x.ParentSequence).ToList<POSAccount.ViewModals.Search.SecurablesRights>();

                    if (item.securablesLst.Count > 0)
                    {
                        lstMenu.Add(item);
                    }
                }

                ViewBag.RoleCode = Role;
            }            

            return View("RoleRights", lstMenu);
        }

        [HttpPost]
        [Route("SaveRights")]
        public ActionResult SaveRights(List<POSAccount.ViewModals.Search.RoleRightsMenu> right)
        {
            try
            {
                var lstRoleRights = new List<POSAccount.Contract.RoleRights>();

                right.Where(r=>r.hasRight==true)
                    .ToList()
                    .ForEach(r=> lstRoleRights.Add( new Contract.RoleRights{ RoleCode = r.RoleCode, SecurableItem = r.SecurableItem} ));

                var result = new POSAccount.BusinessFactory.RoleRightsBO().SaveRoleRights(lstRoleRights);

                /* new code */
                /*
                List<POSAccount.ViewModals.Search.LayoutMenu> lstMenu = new List<POSAccount.ViewModals.Search.LayoutMenu>();
                var roleRights = new POSAccount.BusinessFactory.RoleRightsBO()
                                .GetList(Utility.USERROLE);

                var securablesAll = (List<POSAccount.Contract.Securables>)System.Web.HttpContext.Current.Application["AppSecurables"];

                var securables = securablesAll.Join(roleRights,
                                    sec => sec.SecurableItem,
                                    rig => rig.SecurableItem,
                                    (sec, rig) => new { a = sec, b = rig })
                                .Select(x => new POSAccount.Contract.Securables()
                                {
                                    SecurableItem = x.a.SecurableItem,
                                    GroupID = x.a.GroupID,
                                    Description = x.a.Description,
                                    ActionType = x.a.ActionType,
                                    Link = x.a.Link,
                                    Icon = x.a.Icon
                                })
                                .ToList<POSAccount.Contract.Securables>();


                var menuItems = securablesAll.Where(x => x.ActionType == "TopMenu")
                                    .Select(x => new { securableItem = x.SecurableItem, Icon = x.Icon, GroupId = x.GroupID, Sequence = x.Sequence }).Distinct().ToList().OrderBy(x => x.Sequence).ToList();



                for (var i = 0; i < menuItems.Count; i++)
                {
                    POSAccount.ViewModals.Search.LayoutMenu item = new POSAccount.ViewModals.Search.LayoutMenu();
                    item.MenuName = menuItems[i].securableItem;
                    item.Icon = menuItems[i].Icon;
                    item.securablesLst = securables.Where(x => x.GroupID == menuItems[i].securableItem)
                                                   .ToList<POSAccount.Contract.Securables>();

                    if (item.securablesLst.Count > 0)
                    {
                        lstMenu.Add(item);
                    }
                }
                Session["SsnUserRights"] = lstMenu;
                */
                /* new code */
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }
           // return Json(new { success = true, Message = "Role-Rights saved successfully."});

            return RedirectToAction("RoleRights");
        }

        #endregion
    }    
}