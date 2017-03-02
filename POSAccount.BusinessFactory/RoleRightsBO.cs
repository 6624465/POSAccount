using POSAccount.Contract;
using POSAccount.DataFactory;
using System.Collections.Generic;

namespace POSAccount.BusinessFactory
{
    public class RoleRightsBO
    {
        private RoleRightsDAL   rolerightsDAL;
        public RoleRightsBO()
        {

            rolerightsDAL = new RoleRightsDAL();
        }

        public List<RoleRights> GetList()
        {
            return rolerightsDAL.GetList();
        }


        public List<RoleRights> GetList(string roleCode)
        {
            return rolerightsDAL.GetList(roleCode);
        }


        public List<Securables> GetSecurableItemsList()
        {
            return rolerightsDAL.GetSecurableItemsList();
        }

        public bool SaveRoleRights(RoleRights newItem)
        {

            return rolerightsDAL.Save(newItem);

        }

        public bool SaveRoleRights(List<RoleRights> roleRights)
        {

            return rolerightsDAL.SaveList(roleRights);

        }

        public bool DeleteRoleRights(RoleRights item)
        {
            return rolerightsDAL.Delete(item);
        }

        public bool DeleteAllRightsOfRole(string roleCode)
        {
            return rolerightsDAL.DeleteAllRightsOfRole(roleCode);
        }


        

        public RoleRights GetRoleRights(RoleRights item)
        {
            return (RoleRights)rolerightsDAL.GetItem<RoleRights>(item);
        }

    }
}
