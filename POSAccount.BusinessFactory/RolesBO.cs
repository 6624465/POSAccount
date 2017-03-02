using POSAccount.Contract;
using POSAccount.DataFactory;
using System.Collections.Generic;

namespace POSAccount.BusinessFactory
{
    public class RolesBO
    {
        private RolesDAL   rolesDAL;
        public RolesBO() {

            rolesDAL = new RolesDAL();
        }

        public List<Roles> GetList()
        {
            return rolesDAL.GetList();
        }


        public bool SaveRoles(Roles newItem)
        {

            return rolesDAL.Save(newItem);

        }

        public bool DeleteRoles(Roles item)
        {
            return rolesDAL.Delete(item);
        }

        public Roles GetRoles(Roles item)
        {
            return (Roles)rolesDAL.GetItem<Roles>(item);
        }

    }
}
