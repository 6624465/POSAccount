using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSAccount.Contract;
using POSAccount.DataFactory;

namespace POSAccount.BusinessFactory
{
    public class UsersBO
    {
        private UsersDAL usersDAL;
        public UsersBO()
        {

            usersDAL = new UsersDAL();
        }

        public List<Users> GetList()
        {
            return usersDAL.GetList();
        }


        public bool SaveUsers(Users newItem)
        {

            return usersDAL.Save(newItem);

        }

        public bool DeleteUsers(Users item)
        {
            return usersDAL.Delete(item);
        }

        public Users GetUsers(Users item)
        {
            return (Users)usersDAL.GetItem<Users>(item);
        }


         

    }
}
